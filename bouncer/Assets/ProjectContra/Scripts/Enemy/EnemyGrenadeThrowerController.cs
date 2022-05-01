using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyGrenadeThrowerController : EnemyController
    {
        public float shotInterval = 3f;
        public float xForce = -7f;
        public float yForce = 8f;
        public float zForce = 0f;

        private GameStoreData storeData;
        private Rigidbody rb;
        private GameObject destroyEffect;

        private bool canFireShot = true;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidBodyAndFreezeZ(gameObject);
            destroyEffect = AppResource.instance.enemyGrenadeSmallExplosion;
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
            UnityFn.RunWithInterval(this, shotInterval, canFireShot, (s) => canFireShot = s, () =>
            {
                float x = (closestPlayer.position.x < position.x) ? xForce : -(xForce);
                ThrowGrenade(position, x);
                UnityFn.SetTimeout(this, 0.5f, () => ThrowGrenade(position, x));
                UnityFn.SetTimeout(this, 1f, () => ThrowGrenade(position, x));
            });
        }

        private void ThrowGrenade(Vector3 position, float xToUse)
        {
            EnemyGrenadeController.Spawn(position, xToUse, yForce, zForce, EnemyBulletType.GRENADE);
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