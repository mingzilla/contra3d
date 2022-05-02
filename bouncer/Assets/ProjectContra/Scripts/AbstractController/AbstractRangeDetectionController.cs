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

        protected void RunIfPlayerIsInRange(GameStoreData storeData, float detectionRange, Action<Transform> fn)
        {
            if (!storeData.HasPlayer()) return;
            Vector3 position = transform.position;
            Transform closestPlayer = storeData.GetClosestPlayer(position).inGameTransform;
            if (UnityFn.IsInRange(transform, closestPlayer, detectionRange))
            {
                fn(closestPlayer);
            }
        }
    }
}