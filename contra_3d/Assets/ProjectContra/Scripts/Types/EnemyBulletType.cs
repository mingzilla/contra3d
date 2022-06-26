using System.Collections.Generic;
using BaseUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Types
{
    public class EnemyBulletType
    {
        public static readonly EnemyBulletType BASIC = Create("BASIC", 1, 1f, 10f, 3f, true, false);
        public static readonly EnemyBulletType FOLLOW = Create("FOLLOW", 1, 3f, 8f, 3f, true, false);
        public static readonly EnemyBulletType GRENADE = Create("GRENADE", 1, 3f, 10f, 5f, true, false);
        public static readonly EnemyBulletType PIERCE = Create("PIERCE", 1, 1f, 25f, 2f, false, false);
        public static readonly EnemyBulletType BLAST = Create("BLAST", 1, 3f, 10f, 3f, true, false);
        public static readonly EnemyBulletType CURVED = Create("CURVED", 1, 1f, 12f, 3f, true, false);
        public static readonly EnemyBulletType LASER = Create("LASER", 1, 1f, 25f, 3f, false, true);

        public string name;
        public int damage;
        public float blastRange;
        public float bulletSpeed;
        public float autoDestroyTime;
        public bool destroyWhenHit;
        public bool useCustomCollider;

        static readonly Dictionary<string, EnemyBulletType> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        private EnemyBulletType()
        {
        }

        static List<EnemyBulletType> All()
        {
            return new List<EnemyBulletType>()
            {
                BASIC, FOLLOW, GRENADE, PIERCE, BLAST, CURVED, LASER
            };
        }

        public static EnemyBulletType Create(string name, int damage, float blastRange, float bulletSpeed, float autoDestroyTime, bool destroyWhenHit, bool useCustomCollider)
        {
            EnemyBulletType type = new EnemyBulletType
            {
                name = name,
                damage = damage,
                blastRange = blastRange,
                bulletSpeed = bulletSpeed,
                autoDestroyTime = autoDestroyTime,
                destroyWhenHit = destroyWhenHit,
                useCustomCollider = useCustomCollider,
            };
            return type;
        }

        public static EnemyBulletType GetByName(string name)
        {
            return typeMap[(name)];
        }

        public static EnemyBulletType GetByNameWithDefault(string name)
        {
            return typeMap.ContainsKey(name) ? GetByName(name) : BASIC;
        }

        public LayerMask GetDestructibleLayers()
        {
            return GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});
        }
    }
}