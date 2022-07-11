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
    public class EnemyGroundCannonController : AbstractDestructibleController
    {
        private readonly IntervalState shotInterval = IntervalState.Create(3f);
        [SerializeField] private float detectionRange = 40f;
        [SerializeField] private string bulletName = EnemyBulletType.PIERCE.name;

        private GameStoreData storeData;
        private GameObject destroyEffect;
        private Animator animatorCtrl;
        private EnemyBulletType bulletType;

        private static readonly int isActive = Animator.StringToHash("isActive");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            animatorCtrl = gameObject.GetComponent<Animator>();
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
            bulletType = EnemyBulletType.GetByNameWithDefault(bulletName);
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
            UnityFn.RunWithInterval(AppResource.instance, shotInterval, () =>
            {
                EnemyBasicBulletController.Spawn(position, closestPlayer.position, bulletType, true);
            });
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            AppSfx.Play(AppSfx.instance.bigEnemyDamaged);
            hp -= damage;
            if (hp <= 0)
            {
                AppSfx.PlayRepeatedly(AppSfx.instance.bigEnemyDeath, 3);
                Destroy(gameObject);
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}