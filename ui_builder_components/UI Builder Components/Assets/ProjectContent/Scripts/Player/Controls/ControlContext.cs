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
    }
}