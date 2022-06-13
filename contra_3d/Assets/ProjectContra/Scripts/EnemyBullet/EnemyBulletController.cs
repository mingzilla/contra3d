using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.EnemyBullet
{
    public abstract class EnemyBulletController : MonoBehaviour
    {
        protected void DealDamageToPlayer(Collider other, EnemyBulletType enemyBulletType)
        {
            EnemyUtil.DealDamageToPlayer(transform.position, enemyBulletType.blastRange, enemyBulletType.GetDestructibleLayers(), enemyBulletType.damage, other);
        }

        protected void DestroySelf(GameObject impactEffect, float impactLifeTime)
        {
            Vector3 position = transform.position;
            UnityFn.CreateEffect(impactEffect, position, impactLifeTime); // only if the bullet creates explosion
            Destroy(gameObject);
        }
    }
}