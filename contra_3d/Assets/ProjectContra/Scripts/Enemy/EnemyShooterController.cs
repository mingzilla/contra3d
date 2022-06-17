using System;
using System.Linq;
using BaseUtil.GameUtil.Base;
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
        public float detectionRange = 30f;
        public Vector3 shootPositionDelta = new Vector3(0f, 1f, 0f);
        public Vector3 targetPositionDelta = new Vector3(0f, 1f, 0f);
        public int maxHp = 1;
        public int hp = 1;

        private IntervalState shotIntervalState;
        private EnemyBulletType bulletType;
        private GameStoreData storeData;
        private Rigidbody rb;
        private GameObject destroyEffect;
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
            if (!preventMovement) rb = UnityFn.AddRigidbody(gameObject, true, !moveXZ);
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
            animatorCtrl = gameObject.GetComponent<Animator>();
        }

        void Update()
        {
            bool isInRange = RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                transform.rotation = UnityFn.LookXZ(transform, closestPlayer);
                FireShots(transform.position, closestPlayer);
            });
            if (animatorCtrl) animatorCtrl.SetBool(isShootingKey, isInRange);
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            UnityFn.RunWithInterval(AppResource.instance, shotIntervalState, () =>
            {
                foreach (int i in Enumerable.Range(0, bulletPerShot))
                {
                    UnityFn.SetTimeout(AppResource.instance, i * 0.3f, () =>
                    {
                        if (closestPlayer) SpawnBullet(position, closestPlayer.position);
                    });
                }
            });
        }

        private void SpawnBullet(Vector3 position, Vector3 closestPlayerPosition)
        {
            EnemyBasicBulletController.Spawn(position + shootPositionDelta, closestPlayerPosition + targetPositionDelta, bulletType);
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            AppSfx.PlayAdjusted(AppSfx.instance.enemyDeath);
            hp -= damage;
            if (hp <= 0)
            {
                if (maxHp > 1) AppSfx.PlayRepeatedly(AppSfx.instance.bigEnemyDeath, 3);
                Destroy(gameObject);
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}