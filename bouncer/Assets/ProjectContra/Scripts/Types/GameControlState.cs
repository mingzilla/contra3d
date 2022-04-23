using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Types
{
    public class GameControlState
    {
        public static readonly GameControlState TITLE_SCREEN_MENU = Create("TITLE_SCREEN_MENU");
        public static readonly GameControlState IN_GAME = Create("IN_GAME");

        public string name;

        private static readonly Dictionary<string, GameControlState> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameControlState> All()
        {
            return new List<GameControlState>()
            {
                TITLE_SCREEN_MENU,
                IN_GAME,
            };
        }

        private static GameControlState Create(string name)
        {
            GameControlState layer = new GameControlState
            {
                name = name
            };
            return layer;
        }

        public static GameControlState GetByName(string name)
        {
            return typeMap[(name)];
        }
    }
}