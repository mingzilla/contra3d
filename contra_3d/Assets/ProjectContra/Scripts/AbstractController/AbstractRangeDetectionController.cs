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

        protected void OnDrawGizmosSelected()
        {
            float detectionRange = GetDetectionRange();
            if (detectionRange > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, detectionRange);
            }
        }

        protected bool RunIfPlayerIsInRange(GameStoreData storeData, float detectionRange, Action<Transform> fn)
        {
            if (storeData == null || !storeData.HasPlayer()) return false;
            Vector3 position = transform.position;
            Transform closestPlayer = storeData.GetClosestPlayer(position).inGameTransform;
            bool isInRange = UnityFn.IsInRange(transform, closestPlayer, detectionRange);
            if (isInRange && closestPlayer != null) fn(closestPlayer);
            return isInRange;
        }
    }
}