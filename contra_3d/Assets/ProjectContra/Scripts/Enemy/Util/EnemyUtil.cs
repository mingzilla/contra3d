using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy.Util
{
    public static class EnemyUtil
    {
        public static bool DealDamageToPlayer(Vector3 position, int damage, Collider other)
        {
            bool damagesSomeone = false;
            GameObject obj = other.gameObject;
            if (GameLayer.Matches(obj.layer, GameLayer.PLAYER))
            {
                damagesSomeone = DamagePlayer(obj, position, damage);
            }
            return damagesSomeone;
        }

        public static bool DealDamageToPlayersInRange(Vector3 position, float blastRange, LayerMask destructibleLayers, int damage, Collider other)
        {
            bool damagesSomeone = false;
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER))
            {
                UnityFn.DealDamage(position, blastRange, destructibleLayers, (obj) =>
                {
                    damagesSomeone = DamagePlayer(obj, position, damage);
                });
            }
            return damagesSomeone;
        }

        public static bool DamagePlayer(GameObject player, Vector3 position, int damage)
        {
            // null check to avoid child objects
            CharacterInGameController c1 = player.GetComponentInParent<CharacterInGameController>();
            CharacterInXzGameController c2 = player.GetComponentInParent<CharacterInXzGameController>();
            if (c1 == null && c2 == null) return false;
            if (c1 != null) c1.TakeDamage(position, damage);
            if (c2 != null) c2.TakeDamage(position, damage);
            return true;
        }
    }
}