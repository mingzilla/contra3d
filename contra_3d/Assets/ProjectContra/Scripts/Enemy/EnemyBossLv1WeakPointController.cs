using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv1WeakPointController : AbstractDestructibleController
    {
        private EnemyAttribute attribute;
        public int hp = 20;
        public bool isBroken = false;

        private GameStoreData storeData;
        private Rigidbody rb;
        private GameObject damageEffect;
        private GameObject destroyEffect;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, false, true);
            damageEffect = AppResource.instance.enemyDestroyedSmallExplosion;
            destroyEffect = AppResource.instance.enemyDestroyedBigExplosion;
            attribute = AppResource.instance.enemyAttributeBossLv1WeakPoint;
            hp = attribute.hp;
        }

        void Update()
        {
            // blink the weakpoint
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(damageEffect, position, 5f);
            AppSfx.Play(AppSfx.instance.bigEnemyDamaged);
            hp -= damage;
            if (hp <= 0)
            {
                AppSfx.PlayRepeatedly(AppSfx.instance.bossDeath, 5);
                UnityFn.CreateEffect(destroyEffect, position, 5f);
                isBroken = true;
                Destroy(gameObject);
            }
        }

        public override float GetDetectionRange()
        {
            return -1f;
        }
    }
}