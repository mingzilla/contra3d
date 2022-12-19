using System;
using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Modules.GameMenu.BaseStore.Domain.Types;

namespace ProjectContent.Scripts.UI.Modules.GameMenu.BaseStore.Domain
{
    public class GameMenuActiveItems
    {
        public InGameMenuPanel activeInGameMenuPanel = InGameMenuPanel.NONE;

        public GameMenuItem activeGameMenuItem = GameMenuItem.RESUME; // reset to default selection when returned
        public SettingsScreenMenuItem activeSettingsScreenMenuItem = SettingsScreenMenuItem.SCREEN_RESOLUTION;

        public GameMenuActiveItems down()
        {
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.NONE)) return this;
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.PAUSED_MENU)) return new GameMenuActiveItems {activeGameMenuItem = activeGameMenuItem.down()};
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.CUSTOMISE_PLAYERS)) return this;
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.SETTINGS_SCREEN)) return new GameMenuActiveItems {activeSettingsScreenMenuItem = activeSettingsScreenMenuItem.down()};
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.CONTROLLER_SCREEN)) return this;
            return this;
        }

        public GameMenuActiveItems up()
        {
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.NONE)) return this;
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.PAUSED_MENU)) return new GameMenuActiveItems {activeGameMenuItem = activeGameMenuItem.up()};
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.CUSTOMISE_PLAYERS)) return this;
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.SETTINGS_SCREEN)) return new GameMenuActiveItems {activeSettingsScreenMenuItem = activeSettingsScreenMenuItem.up()};
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.CONTROLLER_SCREEN)) return this;
            return this;
        }

        public GameMenuActiveItems select(Action quitToTitleFn)
        {
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.NONE)) return this;
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.PAUSED_MENU))
            {
                if (ReferenceEquals(activeGameMenuItem, GameMenuItem.RESUME)) return new GameMenuActiveItems();
                if (ReferenceEquals(activeGameMenuItem, GameMenuItem.CUSTOMISE_PLAYERS)) return new GameMenuActiveItems {activeInGameMenuPanel = InGameMenuPanel.CUSTOMISE_PLAYERS};
                if (ReferenceEquals(activeGameMenuItem, GameMenuItem.SETTINGS)) return new GameMenuActiveItems {activeInGameMenuPanel = InGameMenuPanel.SETTINGS_SCREEN};
                if (ReferenceEquals(activeGameMenuItem, GameMenuItem.CONTROL)) return new GameMenuActiveItems {activeInGameMenuPanel = InGameMenuPanel.CONTROLLER_SCREEN};
                if (ReferenceEquals(activeGameMenuItem, GameMenuItem.QUIT_TO_TITLE))
                {
                    quitToTitleFn();
                    return this;
                }
            }
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.CUSTOMISE_PLAYERS)) return this;
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.SETTINGS_SCREEN)) return this;
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.CONTROLLER_SCREEN)) return this;
            return this;
        }

        public GameMenuActiveItems back()
        {
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.NONE)) return this;
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.PAUSED_MENU)) return new GameMenuActiveItems();
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.CUSTOMISE_PLAYERS)) return new GameMenuActiveItems {activeInGameMenuPanel = InGameMenuPanel.PAUSED_MENU};
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.SETTINGS_SCREEN)) return new GameMenuActiveItems {activeInGameMenuPanel = InGameMenuPanel.PAUSED_MENU};
            if (ReferenceEquals(activeInGameMenuPanel, InGameMenuPanel.CONTROLLER_SCREEN)) return new GameMenuActiveItems {activeInGameMenuPanel = InGameMenuPanel.PAUSED_MENU};
            return this;
        }
    }
}