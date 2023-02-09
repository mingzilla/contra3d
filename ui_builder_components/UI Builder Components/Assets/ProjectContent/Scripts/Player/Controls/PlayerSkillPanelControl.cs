using BaseUtil.GameUtil;
using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__SkillSet;
using UnityEngine.InputSystem;

namespace ProjectContent.Scripts.Player.Controls
{
    public class PlayerSkillPanelControl : AbstractControllable
    {
        private PanelForSkillsMono panelForSkillsMono;
        
        public static PlayerSkillPanelControl Create(PlayerMono mono, int playerId)
        {
            PlayerSkillPanelControl item = BaseCreate<PlayerSkillPanelControl>(mono, playerId);
            item.Start();
            item.panelForSkillsMono = PanelForSkillsMono.instance;
            return item;
        }

        protected override void Start()
        {
        }

        public override void FixedUpdate()
        {
        }

        /*------------------------------------------*/

        public override void InputMove(InputAction.CallbackContext context)
        {
            userInput = UserInput.Move(userInput, context);
        }

        public override void KeySelect(InputAction.CallbackContext context)
        {
            mono.SetActiveControl(ControlContext.IN_GAME);
        }

        public override void KeyStart(InputAction.CallbackContext context)
        {
        }

        public override void KeyA(InputAction.CallbackContext context)
        {
        }

        public override void KeyB(InputAction.CallbackContext context)
        {
        }

        public override void KeyX(InputAction.CallbackContext context)
        {
        }

        public override void KeyY(InputAction.CallbackContext context)
        {
        }

        public override void KeyPadUp(InputAction.CallbackContext context)
        {
        }

        public override void KeyPadDown(InputAction.CallbackContext context)
        {
        }

        public override void KeyPadLeft(InputAction.CallbackContext context)
        {
        }

        public override void KeyPadRight(InputAction.CallbackContext context)
        {
        }

        public override void KeyLB(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                panelForSkillsMono.data = new()
                {
                    skillSet1CompData = new PanelForSkillsSkillSetCompData() {isOn = true, slot1 = ElementalType.FIRE, slot2 = ElementalType.LIGHT},
                    skillSet2CompData = new PanelForSkillsSkillSetCompData() {isOn = false, slot1 = ElementalType.WATER, slot2 = ElementalType.LIGHT, slot3 = ElementalType.WATER},
                };
            }
        }

        public override void KeyRB(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                panelForSkillsMono.data = new()
                {
                    skillSet1CompData = new PanelForSkillsSkillSetCompData() {isOn = false, slot1 = ElementalType.FIRE, slot2 = ElementalType.LIGHT},
                    skillSet2CompData = new PanelForSkillsSkillSetCompData() {isOn = true, slot1 = ElementalType.WATER, slot2 = ElementalType.LIGHT, slot3 = ElementalType.WATER},
                };
            }
        }

        public override void KeyLT(InputAction.CallbackContext context)
        {
            if (context.started) userInput.isHoldingLt = true;
            if (context.canceled) userInput.isHoldingLt = false;
        }

        public override void KeyRT(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                userInput.isHoldingRt = true;
            }
            if (context.canceled)
            {
                userInput.isHoldingRt = false;
            }
        }

        public override void KeyboardAnyKey(InputAction.CallbackContext context)
        {
        }
    }
}