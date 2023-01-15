using BaseUtil.GameUtil;
using ProjectContent.Scripts.Types;

namespace ProjectContent.Scripts.Data
{
    public class PlayerSkillAllocation
    {
        public Skill top1;
        public Skill bottom1;
        public Skill left1 = Skill.FIRE_BALL;
        public Skill right1 = Skill.LIGHTENING;

        public Skill top2;
        public Skill bottom2;
        public Skill left2;
        public Skill right2;

        public static Skill GetSkill(PlayerSkillAllocation allocation, UserInput userInput, bool isSet1)
        {
            if (isSet1)
            {
                if (userInput.magicTop) return allocation.top1;
                if (userInput.magicBottom) return allocation.bottom1;
                if (userInput.magicLeft) return allocation.left1;
                if (userInput.magicRight) return allocation.right1;
            }
            else
            {
                if (userInput.magicTop) return allocation.top2;
                if (userInput.magicBottom) return allocation.bottom2;
                if (userInput.magicLeft) return allocation.left2;
                if (userInput.magicRight) return allocation.right2;
            }
            return null;
        }
    }
}