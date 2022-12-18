using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.Types
{
    public class GameMenuPanel : Enum
    {
        public static readonly GameMenuPanel TITLE_MENU = Create("TITLE_MENU");
        public static readonly GameMenuPanel SELECT_SAVE_SLOT_SCREEN = Create("SELECT_SAVE_SLOT_SCREEN");
        public static readonly GameMenuPanel CHARACTER_SELECTION = Create("CHARACTER_SELECTION");
        public static readonly GameMenuPanel SETTINGS_SCREEN = Create("SETTINGS_SCREEN");
        public static readonly GameMenuPanel CONTROLLER_SCREEN = Create("CONTROLLER_SCREEN");

        public string name;

        public static List<GameMenuPanel> All()
        {
            return new List<GameMenuPanel>()
            {
                TITLE_MENU,
                SELECT_SAVE_SLOT_SCREEN,
                CHARACTER_SELECTION,
                SETTINGS_SCREEN,
                CONTROLLER_SCREEN,
            };
        }

        public static Dictionary<string, GameMenuPanel> ItemsMap()
        {
            return All().ToDictionary(x => x.name, x => x);
        }

        private static GameMenuPanel Create(string name)
        {
            return new()
            {
                name = name,
            };
        }
    }
}