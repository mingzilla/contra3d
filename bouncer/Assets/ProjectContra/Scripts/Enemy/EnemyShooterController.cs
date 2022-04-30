using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyShooterController : EnemyController
    {
        public float shotInterval = 3f;

        private GameStoreData storeData;
        private Rigidbody rb;
        private GameObject destroyEffect;

        private bool canFireShot = true;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidBodyAndFreezeZ(gameObject);
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                transform.LookAt(closestPlayer);
                FireShots(transform.position, closestPlayer);
            });
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            if (canFireShot)
            {
                canFireShot = false;
                UnityFn.SetTimeout(this, shotInterval, () =>
                {
                    canFireShot = true;
                    EnemyBasicBulletController.Spawn(position, closestPlayer, EnemyBulletType.BASIC);
                });
            }
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            Destroy(gameObject);
        }

        public override float GetDetectionRange()
        {
            return 50f;
        }
    }
}