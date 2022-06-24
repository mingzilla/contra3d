using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class LavaController : MonoBehaviour
    {
        [SerializeField] private int damage = 1;
        [SerializeField] private int damageInterval = 1;
        private IntervalState damageIntervalState;

        void Start()
        {
            gameObject.layer = GameLayer.GROUND.GetLayer();
            damageIntervalState = IntervalState.Create(damageInterval);
        }

        private void OnTriggerEnter(Collider other)
        {
            DealDamage(other);
        }

        private void OnTriggerStay(Collider other)
        {
            DealDamage(other);
        }

        void DealDamage(Collider other)
        {
            UnityFn.RunWithInterval(this, damageIntervalState, () =>
            {
                Vector3 position = transform.position;
                EnemyUtil.DealDamageToPlayer(position, damage, other);
                AbstractDestructibleController destructible = other.GetComponent<AbstractDestructibleController>();
                if (destructible != null) destructible.TakeDamage(position, damage);
            });
        }
    }
}