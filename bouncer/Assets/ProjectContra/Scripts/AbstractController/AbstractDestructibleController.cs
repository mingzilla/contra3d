using UnityEngine;

namespace ProjectContra.Scripts.AbstractController
{
    public abstract class AbstractDestructibleController : AbstractRangeDetectionController
    {
        public abstract void TakeDamage(Vector3 position, int damage);
    }
}