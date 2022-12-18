using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.UI.Modules.TitleScreen.BaseStore.Domain.Types
{
    public class TitleScreenMenuPanel
    {
        public static readonly TitleScreenMenuPanel TITLE_MENU = Create("TITLE_MENU");
        public static readonly TitleScreenMenuPanel SELECT_SAVE_SLOT_SCREEN = Create("SELECT_SAVE_SLOT_SCREEN");
        public static readonly TitleScreenMenuPanel CHARACTER_SELECTION = Create("CHARACTER_SELECTION");
        public static readonly TitleScreenMenuPanel SETTINGS_SCREEN = Create("SETTINGS_SCREEN");
        public static readonly TitleScreenMenuPanel CONTROLLER_SCREEN = Create("CONTROLLER_SCREEN");

        public string name;

        public static List<TitleScreenMenuPanel> All()
        {
            return new List<TitleScreenMenuPanel>()
            {
                TITLE_MENU,
                SELECT_SAVE_SLOT_SCREEN,
                CHARACTER_SELECTION,
                SETTINGS_SCREEN,
                CONTROLLER_SCREEN,
            };
        }

        public static Dictionary<string, TitleScreenMenuPanel> ItemsMap()
        {
            return All().ToDictionary(x => x.name, x => x);
        }

        private static TitleScreenMenuPanel Create(string name)
        {
            return new()
            {
                name = name,
            };
        }
    }
}