using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.EnemyBullet
{
    public abstract class EnemyBulletController : MonoBehaviour
    {
        protected void DealDamageToPlayer(Collider other, EnemyBulletType enemyBulletType)
        {
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER))
            {
                Vector3 location = transform.position;
                GameFn.DealDamage(location, enemyBulletType.blastRange, enemyBulletType.destructibleLayers, (obj) =>
                {
                    CharacterInGameController character = obj.GetComponentInParent<CharacterInGameController>();
                    if (character != null) character.TakeDamage(location, enemyBulletType.damage); // null check to avoid child objects
                });
            }
        }

        protected void DestroySelf(GameObject impactEffect, float impactLifeTime)
        {
            Vector3 position = transform.position;
            UnityFn.CreateEffect(impactEffect, position, impactLifeTime); // only if the bullet creates explosion
            Destroy(gameObject);
        }
    }
}