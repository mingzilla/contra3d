using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using BaseUtil.GameUtil.Util3D;
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
        private readonly IntervalState shotInterval = IntervalState.Create(3f);
        public float detectionRange = 30f;
        public Vector3 shootPositionDelta = new Vector3(0f, 1f, 0f);
        public Vector3 targetPositionDelta = new Vector3(0f, 1f, 0f);

        private GameStoreData storeData;
        private Rigidbody rb;
        private GameObject destroyEffect;
        private Animator animatorCtrl;
        private static readonly int isActiveKey = Animator.StringToHash("isActive");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, true, true);
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
            animatorCtrl.SetBool(isActiveKey, isInRange);
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            UnityFn.RunWithInterval(AppResource.instance, shotInterval, () =>
            {
                EnemyBasicBulletController.Spawn(position + shootPositionDelta, closestPlayer.position + targetPositionDelta, EnemyBulletType.BASIC);
            });
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
    }
}