using System;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public abstract class EnemyController : MonoBehaviour
    {
        public abstract void TakeDamage(Vector3 position, int damage);
        public abstract float GetDetectionRange();

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetDetectionRange());
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