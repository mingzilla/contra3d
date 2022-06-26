using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.EnemyBullet
{
    public abstract class EnemyBulletController : MonoBehaviour
    {
        protected bool DealDamageToPlayer(Collider other, EnemyBulletType enemyBulletType)
        {
            if (enemyBulletType.blastRange > 1) return EnemyUtil.DealDamageToPlayersInRange(transform.position, enemyBulletType.blastRange, enemyBulletType.GetDestructibleLayers(), enemyBulletType.damage, other);
            if (enemyBulletType.blastRange < 2) return EnemyUtil.DealDamageToPlayer(transform.position, enemyBulletType.damage, other);
            return false;
        }

        protected void DestroySelf(GameObject impactEffect, float impactLifeTime)
        {
            Vector3 position = transform.position;
            UnityFn.CreateEffect(impactEffect, position, impactLifeTime); // only if the bullet creates explosion
            Destroy(gameObject);
        }
    }
}