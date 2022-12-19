using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.UI.Modules.GameMenu.BaseStore.Domain.Types
{
    public class InGameMenuPanel
    {
        public static readonly InGameMenuPanel NONE = Create("NONE"); // hide menu
        public static readonly InGameMenuPanel PAUSED_MENU = Create("PAUSED_MENU");
        public static readonly InGameMenuPanel CUSTOMISE_PLAYERS = Create("CUSTOMISE_PLAYERS");
        public static readonly InGameMenuPanel SETTINGS_SCREEN = Create("SETTINGS_SCREEN");
        public static readonly InGameMenuPanel CONTROLLER_SCREEN = Create("CONTROLLER_SCREEN");

        public string name;

        public static List<InGameMenuPanel> All()
        {
            return new List<InGameMenuPanel>()
            {
                NONE,
                PAUSED_MENU,
                CUSTOMISE_PLAYERS,
                SETTINGS_SCREEN,
                CONTROLLER_SCREEN,
            };
        }

        public static Dictionary<string, InGameMenuPanel> ItemsMap()
        {
            return All().ToDictionary(x => x.name, x => x);
        }

        private static InGameMenuPanel Create(string name)
        {
            return new()
            {
                name = name,
            };
        }
    }
}