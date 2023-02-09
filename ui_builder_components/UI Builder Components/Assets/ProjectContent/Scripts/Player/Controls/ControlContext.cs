using System.Collections.Generic;
using System.Linq;

namespace ProjectContent.Scripts.Player.Controls
{
    public class ControlContext
    {
        public static readonly ControlContext IN_GAME = new() {name = "IN_GAME"};
        public static readonly ControlContext SKILL_PANEL = new() {name = "SKILL_PANEL"};
        public static readonly ControlContext ALLOCATION_PANEL = new() {name = "ALLOCATION_PANEL"};
        public static readonly ControlContext UPGRADE_PANEL = new() {name = "UPGRADE_PANEL"};
        public static readonly ControlContext ACCESSORY_PANEL = new() {name = "ACCESSORY_PANEL"};

        public string name;

        private static readonly Dictionary<string, ControlContext> TYPE_MAP = All().ToDictionary(x => x.name, x => x);

        public static List<ControlContext> All()
        {
            return new List<ControlContext>()
            {
                IN_GAME,
                SKILL_PANEL,
                ALLOCATION_PANEL,
                UPGRADE_PANEL,
                ACCESSORY_PANEL,
            };
        }

        public static ControlContext GetByName(string name)
        {
            return TYPE_MAP[name] ?? IN_GAME;
        }
    }
}