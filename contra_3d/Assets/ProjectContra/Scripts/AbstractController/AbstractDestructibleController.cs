using System;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using UnityEngine;

namespace ProjectContra.Scripts.AbstractController
{
    public abstract class AbstractDestructibleController : AbstractRangeDetectionController
    {
        [SerializeField] public int maxHp = 1;
        [SerializeField] public int hp = 1;

        // After gameObject is destroyed, isBroken is used to check before using any unity function. Otherwise error is thrown when using any Unity functions from the object
        // This is typically needed when a function is run in a SetTimeOut(), an object can be destroyed at the time the function runs.
        public bool isBroken = false;
        public abstract void TakeDamage(Vector3 position, int damage);

        protected void SafeSetTimeOut(float delay, Action fn)
        {
            UnityFn.SetTimeout(this, delay, () =>
            {
                if (!isBroken) fn();
            });
        }

        public void ReduceHpAndCreateEffect(Vector3 position, int damage)
        {
            if (maxHp > 1)
            {
                UnityFn.CreateEffect(AppResource.instance.playerBulletHitEffect, position, 1f);
                AppSfx.PlayAdjusted(AppSfx.instance.bigEnemyDamaged);
                hp -= damage;
                if (hp <= 0)
                {
                    UnityFn.CreateEffect(AppResource.instance.enemyGrenadeSmallExplosion, position, 1f);
                    AppSfx.PlayRepeatedly(AppSfx.instance.bigEnemyDeath, 3);
                    Destroy(gameObject);
                }
            }
            else
            {
                AppSfx.Play(AppSfx.instance.enemyDeath);
                UnityFn.CreateEffect(AppResource.instance.enemyDestroyedSmallExplosion, position, 1f);
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            isBroken = true;
        }
    }
}