using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;

namespace ProjectContra.Scripts.Enemy
{
    public abstract class ActiveWhenVisibleEnemyController : AbstractDestructibleController
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