using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossWeakPointController : AbstractDestructibleController
    {
        [SerializeField] private string gameLayer = GameLayer.ENEMY.name;
        private GameStoreData storeData;
        private GameObject damageEffect;
        private GameObject destroyEffect;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.GetByName(gameLayer).GetLayer();
            damageEffect = AppResource.instance.enemyDestroyedSmallExplosion;
            destroyEffect = AppResource.instance.enemyDestroyedBigExplosion;
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            if (!gameObject.activeSelf) return;
            UnityFn.CreateEffect(damageEffect, position, 5f);
            AppSfx.Play(AppSfx.instance.bigEnemyDamaged);
            hp -= damage;
            if (hp <= 0 && !isBroken)
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