using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.UI.Modules.TitleScreen.BaseStore.Domain.MenuItems
{
    public class SettingsScreenMenuItem
    {
        public static readonly SettingsScreenMenuItem SCREEN_RESOLUTION = new() {name = "SCREEN_RESOLUTION", index = 0};
        public static readonly SettingsScreenMenuItem FULL_SCREEN = new() {name = "FULL_SCREEN", index = 1};
        public static readonly SettingsScreenMenuItem MUSIC_VOLUME = new() {name = "MUSIC_VOLUME", index = 2};
        public static readonly SettingsScreenMenuItem SFX_VOLUME = new() {name = "SFX_VOLUME", index = 3};
        public static readonly SettingsScreenMenuItem BACK = new() {name = "BACK", index = 4};

        public string name;
        public int index;

        public static List<SettingsScreenMenuItem> All()
        {
            return new List<SettingsScreenMenuItem>()
            {
                SCREEN_RESOLUTION,
                FULL_SCREEN,
                MUSIC_VOLUME,
                SFX_VOLUME,
                BACK,
            };
        }

        public SettingsScreenMenuItem down()
        {
            return All().Find(x => x.index == (index + 1)) ?? All().First();
        }

        public SettingsScreenMenuItem up()
        {
            return All().Find(x => x.index == (index - 1)) ?? All().Last();
        }
    }
}