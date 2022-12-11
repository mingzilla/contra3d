using ProjectContent.Scripts.Types;

namespace ProjectContent.Scripts.UI.Modules.EquipSkill.BaseStore.Domain
{
    public class EquipSkillActiveItems
    {
        public int cursorHorizontalIndex; // to show where the moving cursor is
        public int cursorVerticalIndex;

        public GameInputKey editedInput; // XYAB selected to edit skill equipment

        public int activeSkillHorizontalIndex; // to show the current selected skill
        public int activeSkillVerticalIndex;
    }
}