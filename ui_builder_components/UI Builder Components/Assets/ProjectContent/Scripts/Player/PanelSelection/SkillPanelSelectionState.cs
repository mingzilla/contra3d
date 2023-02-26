using BaseUtil.Base;
using BaseUtil.GameUtil;

namespace ProjectContent.Scripts.Player.PanelSelection
{
    public class SkillPanelSelectionState
    {
        public int currentSelectedRowIndex = 0;
        public int currentSelectedColIndex = 0;
        public int rowCount = 6;
        public int colCount = 5;

        public SkillPanelSelectionState Move(UserInput userInput)
        {
            if (userInput.right)
            {
                currentSelectedColIndex = FnVal.GetNextCircularIndex(currentSelectedColIndex, colCount);
            }
            if (userInput.left)
            {
                currentSelectedColIndex = FnVal.GetPreviousCircularIndex(currentSelectedColIndex, colCount);
            }
            if (userInput.up)
            {
                currentSelectedRowIndex = FnVal.GetPreviousCircularIndex(currentSelectedRowIndex, rowCount);
            }
            if (userInput.down)
            {
                currentSelectedRowIndex = FnVal.GetNextCircularIndex(currentSelectedRowIndex, rowCount);
            }
            return this;
        }
    }
}