﻿using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyWalkingShooterController : AbstractDestructibleController
    {
        public int damage = 1;
        public int moveSpeed = 8;
        public float jumpForce = 10f;
        public float detectionRange = 60f;
        [SerializeField] private float closestDistanceToPlayer = 0.5f;
        [SerializeField] private bool useGravity = true;
        [SerializeField] private int bulletsPerShot = 1;
        [SerializeField] private float bulletsDelay = 0.5f;
        [SerializeField] private string bulletName = EnemyBulletType.BASIC.name;

        private GameStoreData storeData;
        private Rigidbody rb;
        private CapsuleCollider theCollider;
        private Animator animatorCtrl;
        private static readonly int isRunning = Animator.StringToHash("isRunning");
        private static readonly int isShootingKey = Animator.StringToHash("isShooting");
        private readonly IntervalState shotInterval = IntervalState.Create(3f);
        public Vector3 shootPositionDelta = new Vector3(0f, 1f, 0f);
        public Vector3 targetPositionDelta = new Vector3(0f, 1f, 0f);
        private bool pauseMovement = false;
        private EnemyBulletType bulletType;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            bool moveXZ = AppResource.instance.GetCurrentScene().moveXZ;
            rb = UnityFn.GetOrAddRigidbody(gameObject, useGravity, !moveXZ);
            animatorCtrl = gameObject.GetComponent<Animator>();
            bulletType = EnemyBulletType.GetByNameWithDefault(bulletName);
        }

        void Update()
        {
            bool isInRange = RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                transform.rotation = UnityFn.LookXZ(transform, closestPlayer);
                FireShots(transform.position, closestPlayer);
                Move(closestPlayer);
            });
            if (!isInRange)
            {
                if (animatorCtrl) animatorCtrl.SetBool(isShootingKey, false);
                if (animatorCtrl) animatorCtrl.SetBool(isRunning, false);
            }
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            UnityFn.RunWithInterval(this, shotInterval, () =>
            {
                if (isBroken || !closestPlayer) return;
                if (animatorCtrl) animatorCtrl.SetBool(isShootingKey, true);
                pauseMovement = true;
                UnityFn.SetTimeout(this, bulletsDelay * bulletsDelay, () =>
                {
                    if (isBroken || !closestPlayer) return;
                    if (animatorCtrl) animatorCtrl.SetBool(isShootingKey, false);
                    pauseMovement = false;
                });
                UnityFn.SetTimeoutWithRepeat(this, bulletsPerShot, bulletsDelay, () =>
                {
                    if (isBroken || !closestPlayer) return;
                    EnemyBasicBulletController.Spawn(position + shootPositionDelta, closestPlayer.position + targetPositionDelta, bulletType, false);
                });
            });
        }

        void Move(Transform closestPlayer)
        {
            if (animatorCtrl) animatorCtrl.SetBool(isRunning, true);
            if (!pauseMovement) MovementUtil.FollowXZTowardsPosition3D(transform, closestPlayer, closestDistanceToPlayer, moveSpeed, Time.deltaTime);
        }

        void OnCollisionEnter(Collision other)
        {
            DealDamage(other);
        }

        void OnCollisionStay(Collision other)
        {
            DealDamage(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            JumpAtTriggerPoint(other);
        }

        void DealDamage(Collision other)
        {
            EnemyUtil.DealDamageToPlayer(transform.position, damage, other.collider);
        }

        private void JumpAtTriggerPoint(Collider other)
        {
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.ENEMY_JUMP_POINT))
            {
                UnityFn.HandleJump(rb, jumpForce);
            }
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            ReduceHpAndCreateEffect(position, damage);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }

        protected void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}