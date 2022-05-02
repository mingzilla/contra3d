using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyGroundCannonController : AbstractDestructibleController
    {
        public float shotInterval = 3f;
        private GameStoreData storeData;
        private Rigidbody rb;
        private GameObject destroyEffect;
        private Animator animatorCtrl;

        private bool canFireShot = true;
        private static readonly int isActive = Animator.StringToHash("isActive");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, true, true);
            animatorCtrl = gameObject.GetComponent<Animator>();
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                animatorCtrl.SetBool(isActive, true); // animate to move up, spend 1.5f
                UnityFn.SetTimeout(this, 1.5f, () => FireShots(transform.position, closestPlayer));
            });
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            UnityFn.RunWithInterval(this, shotInterval, canFireShot, (s) => canFireShot = s, () =>
            {
                EnemyPierceBulletController.Spawn(position, closestPlayer, EnemyBulletType.PIERCE);
            });
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            Destroy(gameObject);
        }

        public override float GetDetectionRange()
        {
            return 40f;
        }
    }
}