using System.Collections.Generic;
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

        private GameStoreData storeData;
        private Rigidbody rb;
        private CapsuleCollider theCollider;
        private LayerMask destructibleLayers;
        private GameObject destroyEffect;
        private Animator animatorCtrl;
        private static readonly int isRunning = Animator.StringToHash("isRunning");
        private static readonly int isShootingKey = Animator.StringToHash("isShooting");
        private readonly IntervalState shotInterval = IntervalState.Create(3f);
        public Vector3 shootPositionDelta = new Vector3(0f, 1f, 0f);
        public Vector3 targetPositionDelta = new Vector3(0f, 1f, 0f);
        private bool pauseMovement = false;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            bool moveXZ = AppResource.instance.GetCurrentScene().moveXZ;
            rb = UnityFn.GetOrAddRigidbody(gameObject, true, !moveXZ);
            destructibleLayers = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
            animatorCtrl = gameObject.GetComponent<Animator>();
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
            UnityFn.RunWithInterval(AppResource.instance, shotInterval, () =>
            {
                if (animatorCtrl) animatorCtrl.SetBool(isShootingKey, true);
                pauseMovement = true;
                UnityFn.SetTimeout(AppResource.instance, 0.5f, () =>
                {
                    if (animatorCtrl) animatorCtrl.SetBool(isShootingKey, false);
                    pauseMovement = false;
                });
                EnemyBasicBulletController.Spawn(position + shootPositionDelta, closestPlayer.position + targetPositionDelta, EnemyBulletType.BASIC, false);
            });
        }

        void Move(Transform closestPlayer)
        {
            if (animatorCtrl) animatorCtrl.SetBool(isRunning, true);
            if (!pauseMovement) MovementUtil.FollowXZTowardsPosition3D(transform, closestPlayer, 0.5f, moveSpeed, Time.deltaTime);
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
                GameFn.HandleJump(rb, jumpForce);
            }
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            AppSfx.PlayAdjusted(AppSfx.instance.enemyDeath);
            Destroy(gameObject);
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