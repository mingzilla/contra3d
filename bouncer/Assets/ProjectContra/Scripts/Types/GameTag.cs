using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;

namespace ProjectContra.Scripts.Types
{
    public class GameTag
    {
        public static readonly GameTag CHARACTER_IN_GAME = Create("CHARACTER_IN_GAME");
        public static readonly GameTag CHARACTER_IN_MENU = Create("CHARACTER_IN_MENU");

        public string name;

        private static readonly Dictionary<string, GameTag> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameTag> All()
        {
            return new List<GameTag>()
            {
                CHARACTER_IN_GAME,
                CHARACTER_IN_MENU,
            };
        }

        private static GameTag Create(string name)
        {
            GameTag layer = new GameTag
            {
                name = name
            };
            return layer;
        }

        public static GameTag GetByName(string name)
        {
            return typeMap[(name)];
        }

        public static void InitOnAwake()
        {
            TagUtil.LogErrorIfTagsNotPresent(Fn.Map(it => it.name, All()));
        }
    }
}