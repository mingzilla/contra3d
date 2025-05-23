﻿using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyShooterController : AbstractDestructibleController
    {
        public float shotInterval = 3f;
        public int bulletPerShot = 1;
        public string bulletName = EnemyBulletType.BASIC.name;
        public bool preventMovement = false;
        [SerializeField] private bool preventRotation = false;
        [SerializeField] private bool enableFreeRotation = false;
        public float detectionRange = 30f;
        public Vector3 shootPositionDelta = new Vector3(0f, 1f, 0f);
        public Vector3 targetPositionDelta = new Vector3(0f, 1f, 0f);

        private IntervalState shotIntervalState;
        private EnemyBulletType bulletType;
        private GameStoreData storeData;
        private Rigidbody rb;
        private Animator animatorCtrl;
        private static readonly int isShootingKey = Animator.StringToHash("isShooting");

        void Start()
        {
            hp = maxHp;
            shotIntervalState = IntervalState.Create(shotInterval);
            bulletType = EnemyBulletType.GetByNameWithDefault(bulletName);
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            bool moveXZ = AppResource.instance.GetCurrentScene().moveXZ;
            if (!preventMovement) rb = UnityFn.GetOrAddRigidbody(gameObject, true, !moveXZ);
            animatorCtrl = gameObject.GetComponent<Animator>();
        }

        void Update()
        {
            bool isInRange = RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                if (!preventRotation)
                {
                    if (!enableFreeRotation) transform.rotation = UnityFn.LookXZ(transform, closestPlayer); // e.g. character with movement animation
                    if (enableFreeRotation) transform.rotation = UnityFn.LookXYZ(transform, closestPlayer); // e.g. mechanical enemies without animation
                }
                FireShots(transform.position, closestPlayer);
            });
            if (animatorCtrl) animatorCtrl.SetBool(isShootingKey, isInRange);
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            UnityFn.RepeatWithInterval(AppResource.instance, shotIntervalState, bulletPerShot, 0.3f, () =>
            {
                if (isBroken) return;
                if (!closestPlayer) return;
                SpawnBullet(position, closestPlayer.position);
            });
        }

        private void SpawnBullet(Vector3 position, Vector3 closestPlayerPosition)
        {
            EnemyBasicBulletController.Spawn(position + shootPositionDelta, closestPlayerPosition + targetPositionDelta, bulletType, false);
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            ReduceHpAndCreateEffect(position, damage);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}