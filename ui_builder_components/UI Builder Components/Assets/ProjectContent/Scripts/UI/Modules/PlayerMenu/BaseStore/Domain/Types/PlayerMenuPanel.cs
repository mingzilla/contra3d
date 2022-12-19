using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.UI.Modules.PlayerMenu.BaseStore.Domain.Types
{
    public class PlayerMenuPanel
    {
        public static readonly PlayerMenuPanel NONE = Create("NONE");
        public static readonly PlayerMenuPanel UPGRADE_PANEL = Create("UPGRADE_PANEL");
        public static readonly PlayerMenuPanel ALLOCATION_PANEL = Create("ALLOCATION_PANEL");
        public static readonly PlayerMenuPanel SKILLS_PANEL = Create("SKILLS_PANEL");
        public static readonly PlayerMenuPanel ACCESSORIES_PANEL = Create("ACCESSORIES_PANEL");

        public string name;

        public static List<PlayerMenuPanel> All()
        {
            return new List<PlayerMenuPanel>()
            {
                NONE,
                UPGRADE_PANEL,
                ALLOCATION_PANEL,
                SKILLS_PANEL,
                ACCESSORIES_PANEL,
            };
        }

        public static Dictionary<string, PlayerMenuPanel> ItemsMap()
        {
            return All().ToDictionary(x => x.name, x => x);
        }

        private static PlayerMenuPanel Create(string name)
        {
            return new()
            {
                name = name,
            };
        }
    }
}