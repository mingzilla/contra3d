using System.Collections.Generic;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv2ElectricController : MonoBehaviour
    {
        public int damage = 2;
        public int range = 4;
        private LayerMask destructibleLayers;

        void Start()
        {
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            destructibleLayers = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});
        }

        void OnCollisionEnter(Collision other)
        {
            DealDamage(other);
        }

        void OnCollisionStay(Collision other)
        {
            DealDamage(other);
        }

        void DealDamage(Collision other)
        {
            EnemyUtil.DealDamageToPlayer(transform.position, damage, other.collider);
        }
    }
}