using ProjectContent.Scripts.Types;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.BaseStore.Domain
{
    public class PanelForSkillsActiveItems
    {
        public int cursorHorizontalIndex; // to show where the moving cursor is
        public int cursorVerticalIndex;

        public GameInputKey editedInput; // XYAB selected to edit skill equipment

        public Skill selectedSkill = Skill.FIRE_BALL; // to show the current selected skill

        // public int activeSkillHorizontalIndex; // to show the current selected skill
        // public int activeSkillVerticalIndex;
    }
}