using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class DestructibleController : AbstractDestructibleController
    {
        private Rigidbody rb;

        private void Start()
        {
            gameObject.layer = GameLayer.DESTRUCTIBLE.GetLayer();
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            ReduceHpAndCreateEffect(position, damage);
        }

        public override float GetDetectionRange()
        {
            return -1;
        }
    }
}