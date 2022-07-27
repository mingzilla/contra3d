using System.Collections.Generic;
using BaseUtil.Base;

namespace ProjectContra.Scripts.Types
{
    public class GameControlState
    {
        public static readonly GameControlState TITLE_SCREEN_MENU = Create("TITLE_SCREEN_MENU"); // control menu
        public static readonly GameControlState TITLE_SCREEN_LOBBY = Create("TITLE_SCREEN_LOBBY"); // character selection on title screen
        public static readonly GameControlState INFO_SCREEN = Create("INFO_SCREEN"); // hit A to skip screen
        public static readonly GameControlState IN_GAME = Create("IN_GAME"); // can move characters around
        public static readonly GameControlState IN_GAME_PAUSED = Create("IN_GAME_PAUSED"); // controlling the UI
        public static readonly GameControlState IN_GAME_LOBBY = Create("IN_GAME_LOBBY"); // character selection in game
        public static readonly GameControlState CANNOT_CONTROL = Create("CANNOT_CONTROL"); // useful when e.g. transitioning to the next level
        public static readonly GameControlState ENDING_SCREEN = Create("ENDING_SCREEN"); // hit A to skip screen

        public string name;

        private static readonly Dictionary<string, GameControlState> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameControlState> All()
        {
            return new List<GameControlState>()
            {
                TITLE_SCREEN_MENU,
                TITLE_SCREEN_LOBBY,
                INFO_SCREEN,
                IN_GAME,
                IN_GAME_PAUSED,
                IN_GAME_LOBBY,
                CANNOT_CONTROL,
                ENDING_SCREEN,
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