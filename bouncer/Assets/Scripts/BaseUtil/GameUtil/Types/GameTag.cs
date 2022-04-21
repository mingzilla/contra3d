using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;

namespace BaseUtil.GameUtil.Types
{
    public class GameTag
    {
        public static GameTag UNIT_ROOT = Create("Unit Root");
        public static GameTag SPRITE = Create("Sprite");

        public string name;

        static Dictionary<string, GameTag> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameTag> All()
        {
            return new List<GameTag>()
            {
                UNIT_ROOT,
                SPRITE,
            };
        }

        private static GameTag Create(string name)
        {
            GameTag layer = new GameTag();
            layer.name = name;
            return layer;
        }

        public static GameTag GetByName(string name)
        {
            return typeMap[(name)];
        }

        public static void InitOnAwake()
        {
            TagUtil.AddTagsIfNotPresent(Fn.Map(it => it.name, All()));
        }
    }
}