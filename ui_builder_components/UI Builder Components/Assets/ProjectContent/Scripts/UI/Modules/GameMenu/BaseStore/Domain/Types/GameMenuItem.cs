using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.UI.Modules.GameMenu.BaseStore.Domain.Types
{
    public class GameMenuItem
    {
        public static readonly GameMenuItem RESUME = new() {name = "RESUME", index = 0};
        public static readonly GameMenuItem CUSTOMISE_PLAYERS = new() {name = "CUSTOMISE_PLAYERS", index = 1};
        public static readonly GameMenuItem SETTINGS = new() {name = "SETTINGS", index = 2};
        public static readonly GameMenuItem CONTROL = new() {name = "CONTROL", index = 3};
        public static readonly GameMenuItem QUIT_TO_TITLE = new() {name = "QUIT_TO_TITLE", index = 4};

        public string name;
        public int index;

        public static List<GameMenuItem> All()
        {
            return new List<GameMenuItem>()
            {
                RESUME,
                CUSTOMISE_PLAYERS,
                SETTINGS,
                CONTROL,
                QUIT_TO_TITLE,
            };
        }

        public GameMenuItem down()
        {
            return All().Find(x => x.index == (index + 1)) ?? All().First();
        }

        public GameMenuItem up()
        {
            return All().Find(x => x.index == (index - 1)) ?? All().Last();
        }
    }
}