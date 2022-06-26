using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;
using Random = System.Random;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyGrenadeThrowingMachineController : AbstractDestructibleController
    {
        private readonly IntervalState shotInterval = IntervalState.Create(3f);
        [SerializeField] private float detectionRange = 40f;
        [SerializeField] private int amountPerThrow = 6;
        [SerializeField] private float yForce = 7f;
        [SerializeField] private Vector3 bulletPositionDelta = new Vector3(-1f, 1f, 0f);
        [SerializeField] private int leftMostDistance = -7;
        [SerializeField] private int rightMostDistance = -1;
        [SerializeField] private int hp = 50;

        private GameStoreData storeData;
        private GameObject destroyEffect;
        private Vector3 position;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
            position = transform.position;
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                UnityFn.RunWithInterval(AppResource.instance, shotInterval, () =>
                {
                    Fn.Times(amountPerThrow, ThrowGrenade);
                });
            });
        }

        private void ThrowGrenade(int i)
        {
            float x = FnVal.RandomFloatBetween(new Random(), leftMostDistance, rightMostDistance);
            EnemyGrenadeController.Spawn(position + bulletPositionDelta, x, yForce, 0f, EnemyBulletType.GRENADE);
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            AppSfx.PlayAdjusted(AppSfx.instance.bigEnemyDamaged);
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