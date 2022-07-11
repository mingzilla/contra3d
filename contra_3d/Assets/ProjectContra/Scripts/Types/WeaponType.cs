using System.Collections.Generic;
using BaseUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Types
{
    public class WeaponType
    {
        public static readonly WeaponType BASIC = Create("BASIC", 1, 1f, 20f, 3f);
        public static readonly WeaponType M = Create("M", 2, 1f, 30f, 3f);
        public static readonly WeaponType BLAST = Create("BLAST", 3, 3f, 40f, 2f);
        public static readonly WeaponType WIDE = Create("WIDE", 1, 1f, 20f, 3f);
        public static readonly WeaponType LASER = Create("LASER", 3, 1f, 20f, 3f);
        public static readonly WeaponType ACCELERATE = Create("ACCELERATE", 1, 1f, 20f, 3f);

        public string name;
        public int damage;
        public float blastRange;
        public float bulletSpeed;
        public float autoDestroyTime;

        static readonly Dictionary<string, WeaponType> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        private WeaponType()
        {
        }

        static List<WeaponType> All()
        {
            return new List<WeaponType>()
            {
                BASIC, M, BLAST, WIDE, LASER, ACCELERATE
            };
        }

        public static WeaponType Create(string name, int damage, float blastRange, float bulletSpeed, float autoDestroyTime)
        {
            WeaponType type = new WeaponType
            {
                name = name,
                damage = damage,
                blastRange = blastRange,
                bulletSpeed = bulletSpeed,
                autoDestroyTime = autoDestroyTime,
            };
            return type;
        }

        public static WeaponType GetByName(string name)
        {
            return typeMap[(name)];
        }

        public LayerMask GetDestructibleLayers()
        {
            // LayerMask can only be in methods, it can't be a field because it can't be part of initialisation
            return GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.DESTRUCTIBLE, GameLayer.ENEMY, GameLayer.ENEMY_INSIDE_WALL, GameLayer.POWER_UP_CONTAINER});
        }
    }
}