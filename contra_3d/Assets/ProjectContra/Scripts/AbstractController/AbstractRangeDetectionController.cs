using System;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.AbstractController
{
    public abstract class AbstractRangeDetectionController : MonoBehaviour
    {
        public abstract float GetDetectionRange();
        public bool isTriggered = false;

        protected void OnDrawGizmosSelected()
        {
            float detectionRange = GetDetectionRange();
            if (detectionRange > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, detectionRange);
            }
        }

        /// <summary>
        /// Execute fn as long as player is in range
        /// </summary>
        /// <returns>isInRange</returns>
        protected bool RunIfPlayerIsInRange(GameStoreData storeData, float detectionRange, Action<Transform> fn)
        {
            if (storeData == null || !storeData.HasPlayer()) return false;
            Vector3 position = transform.position;
            Transform closestPlayer = storeData.GetClosestPlayer(position).inGameTransform;
            bool isInRange = UnityFn.IsInRange(transform, closestPlayer, detectionRange);
            if (isInRange && closestPlayer != null) fn(closestPlayer);
            return isInRange;
        }

        /// <summary>
        /// Once player is in range, fn is executed, and it executes once only
        /// </summary>
        /// <returns>isTriggered</returns>
        protected bool TriggerIfPlayerIsInRange(GameStoreData storeData, float detectionRange, Action<Transform> fn)
        {
            if (isTriggered) return true;
            RunIfPlayerIsInRange(storeData, detectionRange, (transform) =>
            {
                isTriggered = true;
                fn(transform);
            });
            return isTriggered;
        }
    }
}