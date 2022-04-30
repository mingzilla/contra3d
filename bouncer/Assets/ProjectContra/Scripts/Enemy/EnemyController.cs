using System;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
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
    }
}