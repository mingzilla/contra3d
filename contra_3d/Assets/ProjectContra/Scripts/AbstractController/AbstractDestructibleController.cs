using System;
using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using UnityEngine;

namespace ProjectContra.Scripts.AbstractController
{
    public abstract class AbstractDestructibleController : AbstractRangeDetectionController
    {
        [SerializeField] public int maxHp = 1;
        [SerializeField] public int hp = 1;
        [SerializeField] public bool isBig = false; // true to use big explosion when destroyed

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
                    GameObject effect = isBig ? AppResource.instance.enemyDestroyedBigExplosion : AppResource.instance.enemyGrenadeSmallExplosion;
                    UnityFn.CreateEffect(effect, position, 1f);
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

        public static bool AreAllBroken<T>(IEnumerable<T> controllers) where T : AbstractDestructibleController
        {
            return Fn.All(g => g.isBroken, new List<T>(controllers));
        }

        public static void AllTakeDamage<T>(IEnumerable<T> controllers, int damage) where T : AbstractDestructibleController
        {
            foreach (T controller in controllers)
            {
                controller.TakeDamage(controller.gameObject.transform.position, damage);
            }
        }

        private void OnDestroy()
        {
            isBroken = true;
        }
    }
}