using System.Collections.Generic;
using BaseUtil.GameUtil.Types;

namespace ProjectContent.Scripts.Types
{
    public class SelectionStateForSkillPanel : UiItemSelectionState
    {
        public static readonly UiItemSelectionState IS_SET_1_A_ON = Create("IS_SET_1_A_ON");
        public static readonly UiItemSelectionState IS_SET_1_B_ON = Create("IS_SET_1_B_ON");
        public static readonly UiItemSelectionState IS_SET_1_X_ON = Create("IS_SET_1_X_ON");
        public static readonly UiItemSelectionState IS_SET_1_Y_ON = Create("IS_SET_1_Y_ON");

        public static readonly UiItemSelectionState IS_SET_2_A_ON = Create("IS_SET_2_A_ON");
        public static readonly UiItemSelectionState IS_SET_2_B_ON = Create("IS_SET_2_B_ON");
        public static readonly UiItemSelectionState IS_SET_2_X_ON = Create("IS_SET_2_X_ON");
        public static readonly UiItemSelectionState IS_SET_2_Y_ON = Create("IS_SET_2_Y_ON");
        
        static List<UiItemSelectionState> All()
        {
            return new List<UiItemSelectionState>()
            {
                IS_VISIBLE,
                IS_INVISIBLE,
                IS_ENABLED,
                IS_DISABLED,
                IS_SELECTED,
                IS_SET_1_A_ON,
                IS_SET_1_B_ON,
                IS_SET_1_X_ON,
                IS_SET_1_Y_ON,
                IS_SET_2_A_ON,
                IS_SET_2_B_ON,
                IS_SET_2_X_ON,
                IS_SET_2_Y_ON,
            };
        }
    }
}