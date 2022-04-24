using System.Collections.Generic;
using BaseUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Types
{
    public class WeaponType
    {
        public static readonly WeaponType BASIC = Create("BASIC", 1, 1f);
        public static readonly WeaponType BLAST = Create("BLAST", 5, 3f);
        public static readonly WeaponType WIDE = Create("WIDE", 2, 1f);
        public static readonly WeaponType LASER = Create("LASER", 5, 1f);
        public static readonly WeaponType ACCELERATE = Create("ACCELERATE", 1, 1f);

        public string name;
        public int damage;
        public float blastRange;

        public LayerMask destructibleLayers = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.DESTRUCTIBLE, GameLayer.ENEMY});

        static readonly Dictionary<string, WeaponType> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        private WeaponType()
        {
        }

        static List<WeaponType> All()
        {
            return new List<WeaponType>()
            {
                BASIC, BLAST, WIDE, LASER, ACCELERATE
            };
        }

        public static WeaponType Create(string name, int damage, float blastRange)
        {
            WeaponType type = new WeaponType
            {
                name = name,
                damage = damage,
                blastRange = blastRange
            };
            return type;
        }

        public static WeaponType GetByName(string name)
        {
            return typeMap[(name)];
        }
    }
}