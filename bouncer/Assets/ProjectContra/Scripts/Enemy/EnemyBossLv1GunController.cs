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
    public class EnemyBossLv1GunController : AbstractDestructibleController
    {
        private readonly IntervalState shotInterval = IntervalState.Create(5f);
        public int fireDelay = 0;
        public int hp = 5;
        public bool isBroken = false;

        private GameStoreData storeData;
        private Rigidbody rb;
        private GameObject destroyEffect;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, false, true);
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                transform.LookAt(closestPlayer);
                UnityFn.SetTimeout(this, fireDelay, () => FireShots(transform.position, closestPlayer));
            });
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            UnityFn.RunWithInterval(this, shotInterval, () =>
            {
                EnemyBasicBulletController.Spawn(position, closestPlayer, EnemyBulletType.BASIC);
            });
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            hp -= damage;
            if (hp <= 0)
            {
                isBroken = true;
                Destroy(gameObject);
            }
        }

        public override float GetDetectionRange()
        {
            return 30f;
        }
    }
}