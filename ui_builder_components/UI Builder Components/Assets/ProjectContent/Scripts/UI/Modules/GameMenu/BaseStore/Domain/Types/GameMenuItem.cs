using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.UI.Modules.GameMenu.BaseStore.Domain.Types
{
    public class GameMenuItem
    {
        public static readonly GameMenuItem RESUME = new() {name = "RESUME", index = 0};
        public static readonly GameMenuItem SETTINGS = new() {name = "SETTINGS", index = 1};
        public static readonly GameMenuItem CONTROL = new() {name = "CONTROL", index = 2};
        public static readonly GameMenuItem EXIT = new() {name = "EXIT", index = 3};

        public string name;
        public int index;

        public static List<GameMenuItem> All()
        {
            return new List<GameMenuItem>()
            {
                RESUME,
                SETTINGS,
                CONTROL,
                EXIT,
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