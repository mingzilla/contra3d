using System;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.AbstractController
{
    public abstract class AbstractDestructibleController : AbstractRangeDetectionController
    {
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
    }
}