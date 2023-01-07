using System.Collections;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace BaseUtil.GameUtil.Util3D
{
    /// <summary>
    /// Used to apply modifier for a short amount of time.
    /// e.g. when attacking, player moves slower, after 0.25 seconds, the player can move with their normal speed
    /// </summary>
    public class MoveSpeedModifier
    {
        public float speedModifier = 1f;

        private IEnumerator speedModifierCoroutine;
        private float delay = 0.25f;
        private float modifiedValue = 0.05f;

        public static MoveSpeedModifier Create()
        {
            return new MoveSpeedModifier();
        }

        public static MoveSpeedModifier CreateWith(float delay, float modifiedValue)
        {
            return new MoveSpeedModifier()
            {
                delay = delay,
                modifiedValue = modifiedValue,
            };
        }

        public void TemporarilyApplyModifier(MonoBehaviour controller)
        {
            UnityFn.CancelTimeout(controller, speedModifierCoroutine);
            speedModifier = modifiedValue;
            speedModifierCoroutine = UnityFn.SetTimeout(controller, delay, () => speedModifier = 1f);
        }

        public void ToggleIdle(bool isIdle)
        {
            if (isIdle) speedModifier = 0;
            if (!isIdle) speedModifier = 1;
        }

        public bool CanUseMoveAnimation()
        {
            return speedModifier != 0;
        }
    }
}