using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyTankController : AbstractDestructibleController
    {
        private readonly IntervalState shotInterval = IntervalState.Create(2f);
        public float detectionRange = 30f;
        public Vector3 shootPositionDelta = new Vector3(0f, 0.4f, -0.5f);
        public Vector3 targetPositionDelta = new Vector3(0f, 0f, 0f);

        private GameStoreData storeData;
        private GameObject destroyEffect;
        private bool isActive = true;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
        }

        void Update()
        {
            if (!isActive) return;
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), FireShots);
        }

        void FireShots(Transform closestPlayer)
        {
            Vector3 shootPosition = transform.position + shootPositionDelta;
            Vector3 targetPosition = shootPosition + targetPositionDelta;
            UnityFn.RunWithInterval(this, shotInterval, () =>
            {
                EnemyBasicBulletController.Spawn(shootPosition, (targetPosition + new Vector3(0f, 0f, -1f)), EnemyBulletType.PIERCE, false);
                EnemyBasicBulletController.Spawn(shootPosition, (targetPosition + new Vector3(-0.2f, 0f, -1f)), EnemyBulletType.PIERCE, false);
                EnemyBasicBulletController.Spawn(shootPosition, (targetPosition + new Vector3(0.2f, 0f, -1f)), EnemyBulletType.PIERCE, false);
            });
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            if (!isActive) return;
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            AppSfx.Play(AppSfx.instance.bigEnemyDamaged);
            hp -= damage;
            if (hp <= 0)
            {
                AppSfx.PlayRepeatedly(AppSfx.instance.bigEnemyDeath, 3);
                isActive = false;
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}