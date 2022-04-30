using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public abstract class ActiveWhenVisibleEnemyController : EnemyController
    {
        protected void OnBecameVisible()
        {
            UnityFn.SetActive(gameObject, Fn.DoNothing);
        }

        protected void OnBecameInvisible()
        {
            UnityFn.SetInactive(gameObject, Fn.DoNothing);
        }
    }
}