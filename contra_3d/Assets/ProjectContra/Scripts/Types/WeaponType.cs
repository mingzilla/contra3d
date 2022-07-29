using System.Collections.Generic;
using BaseUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Types
{
    public class WeaponType
    {
        public static readonly WeaponType BASIC = Create("BASIC", 1, 1f, 20f, true, 3f);
        public static readonly WeaponType M = Create("M", 2, 1f, 30f, true, 3f);
        public static readonly WeaponType BLAST = Create("BLAST", 2, 3f, 40f, true, 2f);
        public static readonly WeaponType WIDE = Create("WIDE", 1, 1f, 20f, true, 3f);
        public static readonly WeaponType LASER = Create("LASER", 1, 1f, 30f, false, 3f);
        public static readonly WeaponType ACCELERATE = Create("ACCELERATE", 1, 1f, 20f, true, 3f);

        public string name;
        public int damage;
        public float blastRange;
        public float bulletSpeed;
        public bool destroyWhenHit;
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

        public static WeaponType Create(string name, int damage, float blastRange, float bulletSpeed, bool destroyWhenHit, float autoDestroyTime)
        {
            WeaponType type = new WeaponType
            {
                name = name,
                damage = damage,
                blastRange = blastRange,
                bulletSpeed = bulletSpeed,
                destroyWhenHit = destroyWhenHit,
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

        public Vector3 GetBulletPositionDelta(float inputFixedHorizontal, bool isProne)
        {
            if (this == WIDE) return new Vector3(inputFixedHorizontal, 1.2f, 0f);
            return new Vector3(inputFixedHorizontal, (isProne ? 0.6f : 1f), 0f);
        }

        public Vector3 GetBulletPositionXzDelta(float inputFixedHorizontal)
        {
            if (this == WIDE) return new Vector3(inputFixedHorizontal, 1.2f, 0f);
            return new Vector3(inputFixedHorizontal, 1f, 0f);
        }
    }
}