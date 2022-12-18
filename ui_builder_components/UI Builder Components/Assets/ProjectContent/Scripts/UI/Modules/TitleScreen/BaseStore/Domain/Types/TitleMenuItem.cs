using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.UI.Modules.TitleScreen.BaseStore.Domain.Types
{
    public class TitleMenuItem
    {
        public static readonly TitleMenuItem START = new() {name = "START", index = 0};
        public static readonly TitleMenuItem SETTINGS = new() {name = "SETTINGS", index = 1};
        public static readonly TitleMenuItem CONTROL = new() {name = "CONTROL", index = 2};
        public static readonly TitleMenuItem EXIT = new() {name = "EXIT", index = 3};

        public string name;
        public int index;

        public static List<TitleMenuItem> All()
        {
            return new List<TitleMenuItem>()
            {
                START,
                SETTINGS,
                CONTROL,
                EXIT,
            };
        }

        public TitleMenuItem down()
        {
            return All().Find(x => x.index == (index + 1)) ?? All().First();
        }

        public TitleMenuItem up()
        {
            return All().Find(x => x.index == (index - 1)) ?? All().Last();
        }
    }
}