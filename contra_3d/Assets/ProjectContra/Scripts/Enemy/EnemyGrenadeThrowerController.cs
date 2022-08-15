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
    public class EnemyGrenadeThrowerController : AbstractDestructibleController
    {
        private readonly IntervalState shotInterval = IntervalState.Create(3f);
        public float xForce = -7f;
        public float yForce = 8f;
        public float zForce = 0f;
        [SerializeField] private float detectionRange = 50f;

        private GameStoreData storeData;
        private Rigidbody rb;
        private GameObject destroyEffect;
        private Animator animatorCtrl;
        private static readonly int isActiveKey = Animator.StringToHash("isActive");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.GetOrAddRigidbody(gameObject, true, true);
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
            animatorCtrl = gameObject.GetComponent<Animator>();
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                transform.rotation = UnityFn.LookXZ(transform, closestPlayer);
                FireShots(transform.position, closestPlayer);
            });
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            UnityFn.RunWithInterval(this, shotInterval, () =>
            {
                float x = (closestPlayer.position.x < position.x) ? xForce : -(xForce);
                ThrowGrenade(position, x);
                UnityFn.SetTimeout(this, 0.5f, () => ThrowGrenade(position, x));
                UnityFn.SetTimeout(this, 1f, () => ThrowGrenade(position, x));
            });
        }

        private void ThrowGrenade(Vector3 position, float xToUse)
        {
            animatorCtrl.SetBool(isActiveKey, true);
            UnityFn.SetTimeout(this, 0.4f, () =>
            {
                if (animatorCtrl) animatorCtrl.SetBool(isActiveKey, false);
            });
            EnemyGrenadeController.Spawn(position, xToUse, yForce, zForce, EnemyBulletType.GRENADE);
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            Destroy(gameObject);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}