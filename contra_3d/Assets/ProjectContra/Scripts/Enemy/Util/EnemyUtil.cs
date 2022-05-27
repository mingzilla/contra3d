using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy.Util
{
    public static class EnemyUtil
    {
        public static void DealDamageToPlayer(Vector3 position, float blastRange, LayerMask destructibleLayers, int damage, Collider other)
        {
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER))
            {
                GameFn.DealDamage(position, blastRange, destructibleLayers, (obj) =>
                {
                    CharacterInGameController character = obj.GetComponentInParent<CharacterInGameController>();
                    if (character != null) character.TakeDamage(position, damage); // null check to avoid child objects
                });
            }
        }
    }
}