using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy.Util
{
    public static class EnemyUtil
    {
        public static void DealDamageToPlayer(Vector3 position, int damage, Collider other)
        {
            GameObject obj = other.gameObject;
            if (GameLayer.Matches(obj.layer, GameLayer.PLAYER))
            {
                // null check to avoid child objects
                obj.GetComponentInParent<CharacterInGameController>()?.TakeDamage(position, damage);
                obj.GetComponentInParent<CharacterInXzGameController>()?.TakeDamage(position, damage);
            }
        }

        public static void DealDamageToPlayersInRange(Vector3 position, float blastRange, LayerMask destructibleLayers, int damage, Collider other)
        {
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER))
            {
                GameFn.DealDamage(position, blastRange, destructibleLayers, (obj) =>
                {
                    // null check to avoid child objects
                    obj.GetComponentInParent<CharacterInGameController>()?.TakeDamage(position, damage);
                    obj.GetComponentInParent<CharacterInXzGameController>()?.TakeDamage(position, damage);
                });
            }
        }
    }
}