using BaseUtil.GameUtil;
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
    public class EnemyGroundShooterController : AbstractDestructibleController
    {
        private readonly IntervalState shotInterval = IntervalState.Create(3f);
        private EnemyAttribute attribute;
        public int hp = 10;
        [SerializeField] private float detectionRange = 40f;
        [SerializeField] private string bulletName = EnemyBulletType.PIERCE.name;
        [SerializeField] private float targetDeltaY = 2f;
        [SerializeField] private int bulletPerShot = 2;

        private GameStoreData storeData;
        private Vector3 targetPosition;
        private EnemyBulletType bulletType;
        private int bulletMoveXValue = -1;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            attribute = AppResource.instance.enemyAttributeGroundCannon;
            hp = attribute.hp;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            bulletType = EnemyBulletType.GetByNameWithDefault(bulletName);
            targetPosition = transform.position + new Vector3(0f, targetDeltaY, 0f);
        }

        void Update()
        {
            TriggerIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                bulletMoveXValue = (closestPlayer.position.x < transform.position.x) ? -1 : 1;
            });
            if (isTriggered)
            {
                MovementUtil.MoveToPositionOverTime(transform, targetPosition, 0.1f, 4);
                RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
                {
                    if (isBroken) return;
                    UnityFn.SetTimeout(this, 1.5f, () => FireShots(transform.position, closestPlayer));
                });
            }
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            if (isBroken) return;
            UnityFn.RepeatWithInterval(this, shotInterval, bulletPerShot, 0.2f, () =>
            {
                if (isBroken) return;
                if (!closestPlayer) return;
                EnemyBasicBulletController bullet = EnemyBasicBulletController.Spawn(position, closestPlayer.position, bulletType, true);
                bullet.moveXValue = bulletMoveXValue;
            });
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(AppResource.instance.enemyDestroyedSmallExplosion, position, 1f);
            AppSfx.Play(AppSfx.instance.bigEnemyDamaged);
            hp -= damage;
            if (hp <= 0)
            {
                AppSfx.PlayRepeatedly(AppSfx.instance.bigEnemyDeath, 3);
                isBroken = true;
                Destroy(gameObject);
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}