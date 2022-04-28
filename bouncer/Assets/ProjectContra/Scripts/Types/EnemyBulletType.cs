using System.Collections.Generic;
using BaseUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Types
{
    public class EnemyBulletType
    {
        public static readonly EnemyBulletType BASIC = Create("BASIC", 1, 1f, 10f, 3f);
        public static readonly EnemyBulletType BLAST = Create("BLAST", 1, 3f, 10f, 3f);
        public static readonly EnemyBulletType WIDE = Create("WIDE", 1, 1f, 10f, 3f);
        public static readonly EnemyBulletType LASER = Create("LASER", 1, 1f, 10f, 3f);

        public string name;
        public int damage;
        public float blastRange;
        public float bulletSpeed;
        public float autoDestroyTime;

        public LayerMask destructibleLayers = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});

        static readonly Dictionary<string, EnemyBulletType> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        private EnemyBulletType()
        {
        }

        static List<EnemyBulletType> All()
        {
            return new List<EnemyBulletType>()
            {
                BASIC, BLAST, WIDE, LASER
            };
        }

        public static EnemyBulletType Create(string name, int damage, float blastRange, float bulletSpeed, float autoDestroyTime)
        {
            EnemyBulletType type = new EnemyBulletType
            {
                name = name,
                damage = damage,
                blastRange = blastRange,
                bulletSpeed = bulletSpeed,
                autoDestroyTime = autoDestroyTime,
            };
            return type;
        }

        public static EnemyBulletType GetByName(string name)
        {
            return typeMap[(name)];
        }
    }
}