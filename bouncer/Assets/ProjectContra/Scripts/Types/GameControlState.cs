using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Types
{
    public class GameControlState
    {
        public static readonly GameControlState TITLE_SCREEN_MENU = Create("TITLE_SCREEN_MENU"); // control menu
        public static readonly GameControlState LOBBY_SCREEN = Create("LOBBY_SCREEN"); // control character selection
        public static readonly GameControlState INFO_SCREEN = Create("INFO_SCREEN"); // hit A to skip screen
        public static readonly GameControlState IN_GAME = Create("IN_GAME"); // can move characters around
        public static readonly GameControlState CANNOT_CONTROL = Create("CANNOT_CONTROL"); // useful when e.g. transitioning to the next level

        public string name;

        private static readonly Dictionary<string, GameControlState> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameControlState> All()
        {
            return new List<GameControlState>()
            {
                TITLE_SCREEN_MENU,
                LOBBY_SCREEN,
                INFO_SCREEN,
                IN_GAME,
                CANNOT_CONTROL,
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