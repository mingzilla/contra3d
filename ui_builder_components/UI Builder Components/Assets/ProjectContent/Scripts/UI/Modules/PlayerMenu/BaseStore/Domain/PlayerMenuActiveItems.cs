using ProjectContent.Scripts.UI.Modules.PlayerMenu.BaseStore.Domain.ActiveItems;
using ProjectContent.Scripts.UI.Modules.PlayerMenu.BaseStore.Domain.Types;

namespace ProjectContent.Scripts.UI.Modules.PlayerMenu.BaseStore.Domain
{
    public class PlayerMenuActiveItems
    {
        public PlayerMenuPanel activePlayerMenuPanel = PlayerMenuPanel.NONE;

        public PlayerMenuUpgradePanelActiveItems upgradePanelActiveItems;
        public PlayerMenuAllocationPanelActiveItems allocationPanelActiveItems;
        public PlayerMenuSkillsPanelActiveItems skillsPanelActiveItems;
        public PlayerMenuAccessoriesPanelActiveItems accessoriesPanelActiveItems;
    }
}