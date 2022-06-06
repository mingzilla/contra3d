using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.EnemyBullet
{
    public class EnemyGrenadeExplosionController : EnemyBulletController
    {
        private void OnTriggerEnter(Collider other)
        {
            DealDamageToPlayer(other, EnemyBulletType.GRENADE);
        }
    }
}