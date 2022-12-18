using System;
using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Modules.TitleScreen.BaseStore.Domain.Types;

namespace ProjectContent.Scripts.UI.Modules.TitleScreen.BaseStore.Domain
{
    public class TitleScreenActiveItems
    {
        public TitleScreenMenuPanel activeTitleScreenMenuPanel = TitleScreenMenuPanel.TITLE_MENU;

        public TitleMenuItem activeTitleMenuItem = TitleMenuItem.START; // reset to default selection when returned
        public int activeSaveSlot = 0; // index
        public SettingsScreenMenuItem activeSettingsScreenMenuItem = SettingsScreenMenuItem.SCREEN_RESOLUTION;

        public TitleScreenActiveItems down()
        {
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.TITLE_MENU)) return new TitleScreenActiveItems {activeTitleMenuItem = activeTitleMenuItem.down()};
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.SELECT_SAVE_SLOT_SCREEN)) return new TitleScreenActiveItems {activeSaveSlot = (activeSaveSlot + 1)};
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.SETTINGS_SCREEN)) return new TitleScreenActiveItems {activeSettingsScreenMenuItem = activeSettingsScreenMenuItem.down()};
            return this;
        }

        public TitleScreenActiveItems up()
        {
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.TITLE_MENU)) return new TitleScreenActiveItems {activeTitleMenuItem = activeTitleMenuItem.up()};
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.SELECT_SAVE_SLOT_SCREEN)) return new TitleScreenActiveItems {activeSaveSlot = (activeSaveSlot - 1)};
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.SETTINGS_SCREEN)) return new TitleScreenActiveItems {activeSettingsScreenMenuItem = activeSettingsScreenMenuItem.up()};
            return this;
        }

        public TitleScreenActiveItems select(Action exitGameFn)
        {
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.TITLE_MENU))
            {
                if (ReferenceEquals(activeTitleMenuItem, TitleMenuItem.START)) return new TitleScreenActiveItems {activeTitleScreenMenuPanel = TitleScreenMenuPanel.SELECT_SAVE_SLOT_SCREEN};
                if (ReferenceEquals(activeTitleMenuItem, TitleMenuItem.SETTINGS)) return new TitleScreenActiveItems {activeTitleScreenMenuPanel = TitleScreenMenuPanel.SETTINGS_SCREEN};
                if (ReferenceEquals(activeTitleMenuItem, TitleMenuItem.CONTROL)) return new TitleScreenActiveItems {activeTitleScreenMenuPanel = TitleScreenMenuPanel.CONTROLLER_SCREEN};
                if (ReferenceEquals(activeTitleMenuItem, TitleMenuItem.EXIT))
                {
                    exitGameFn();
                    return this;
                }
            }
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.SELECT_SAVE_SLOT_SCREEN))
            {
                return new TitleScreenActiveItems {activeTitleScreenMenuPanel = TitleScreenMenuPanel.CHARACTER_SELECTION, activeSaveSlot = activeSaveSlot};
            }
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.CHARACTER_SELECTION)) return this;
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.SETTINGS_SCREEN)) return this;
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.CONTROLLER_SCREEN)) return this;
            return this;
        }

        public TitleScreenActiveItems back()
        {
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.TITLE_MENU)) return this;
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.SELECT_SAVE_SLOT_SCREEN)) return new TitleScreenActiveItems();
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.CHARACTER_SELECTION)) return new TitleScreenActiveItems {activeTitleScreenMenuPanel = TitleScreenMenuPanel.SELECT_SAVE_SLOT_SCREEN, activeSaveSlot = activeSaveSlot};
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.SETTINGS_SCREEN)) return new TitleScreenActiveItems {activeTitleMenuItem = TitleMenuItem.SETTINGS};
            if (ReferenceEquals(activeTitleScreenMenuPanel, TitleScreenMenuPanel.CONTROLLER_SCREEN)) return new TitleScreenActiveItems {activeTitleMenuItem = TitleMenuItem.CONTROL};
            return this;
        }
    }
}