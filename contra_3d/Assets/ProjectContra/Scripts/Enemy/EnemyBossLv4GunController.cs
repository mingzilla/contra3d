using BaseUtil.Base;
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
    public class EnemyBossLv4GunController : AbstractDestructibleController
    {
        private readonly IntervalState shotInterval = IntervalState.Create(5f);
        public int bulletPerShot = 3;
        public float fireDelay = 0f;
        public int hp = 20;
        public bool canShoot = false;
        public bool isBroken = false;

        private GameObject destroyEffect;

        void Start()
        {
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
        }

        void Update()
        {
            if (!canShoot || isBroken) return;
            UnityFn.SetTimeout(this, fireDelay, FireShots);
        }

        void FireShots()
        {
            UnityFn.RunWithInterval(AppResource.instance, shotInterval, () =>
            {
                Fn.Times(bulletPerShot, (i) =>
                {
                    UnityFn.SetTimeout(AppResource.instance, i * 0.1f, () =>
                    {
                        Vector3 position = transform.position;
                        Vector3 targetPosition = position + Vector3.down;
                        AppSfx.Play(AppSfx.instance.enemyLaser);
                        EnemyBasicBulletController.Spawn(position, targetPosition, EnemyBulletType.LASER, false);
                    });
                });
            });
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            AppSfx.PlayAdjusted(AppSfx.instance.bigEnemyDamaged);
            hp -= damage;
            if (hp <= 0)
            {
                AppSfx.PlayRepeatedly(AppSfx.instance.enemyDeath, 3);
                isBroken = true;
                Destroy(gameObject);
            }
        }

        public override float GetDetectionRange()
        {
            return -1;
        }
    }
}