using System;
using ProjectContent.Scripts.Types;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__SkillSet
{
    /// <summary>
    /// CS representation of the component, turns all changeable content to variables 
    /// </summary>
    public class PanelForSkillsSkillSetComp
    {
        private VisualElement root; // root of top level

        private VisualElement compRoot; // root of current component
        private string compRootName; // panel-for-skills__skill-set__set-1

        private VisualElement skillSetElement;
        private int setId;

        private VisualElement skillSetStaffElement;

        private VisualElement skillSetSlot1Element;
        private VisualElement skillSetSlot2Element;
        private VisualElement skillSetSlot3Element;
        private VisualElement skillSetSlot4Element;

        private VisualElement skillSetBtnIndicatorElement;
        private Label skillSetBtnTextElement;

        private PanelForSkillsSkillSetCompData data;

        public static PanelForSkillsSkillSetComp Create(VisualElement root, int setId)
        {
            string compRootName = setId == 1 ? "panel-for-skills__skill-set__set-1" : "panel-for-skills__skill-set__set-2";
            VisualElement compRoot = root.Q<VisualElement>(compRootName);

            VisualElement skillSetElement = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set");
            VisualElement skillSetStaffElement = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__staff");

            VisualElement skillSetSlot1Element = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__slot-1");
            VisualElement skillSetSlot2Element = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__slot-2");
            VisualElement skillSetSlot3Element = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__slot-3");
            VisualElement skillSetSlot4Element = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__slot-4");

            VisualElement skillSetBtnIndicatorElement = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__btn-indicator");
            Label skillSetBtnTextElement = compRoot.Q<Label>("ve__panel-for-skills__skill-set__set__btn-text");

            return new()
            {
                root = root,

                compRoot = compRoot,
                compRootName = compRootName,

                skillSetElement = skillSetElement,
                setId = setId,

                skillSetStaffElement = skillSetStaffElement,

                skillSetSlot1Element = skillSetSlot1Element,
                skillSetSlot2Element = skillSetSlot2Element,
                skillSetSlot3Element = skillSetSlot3Element,
                skillSetSlot4Element = skillSetSlot4Element,
                
                skillSetBtnIndicatorElement = skillSetBtnIndicatorElement,
                skillSetBtnTextElement = skillSetBtnTextElement,
            };
        }

        public void InitData(PanelForSkillsSkillSetCompData dataIn)
        {
            data = dataIn;
        }

        public void BuildOnStart()
        {
            skillSetStaffElement.RemoveFromClassList("staff-equipment-1");
            skillSetStaffElement.AddToClassList(setId == 1 ? "staff-equipment-1" : "staff-equipment-2");

            skillSetBtnIndicatorElement.RemoveFromClassList("set-1-bg");
            skillSetBtnIndicatorElement.AddToClassList(setId == 1 ? "set-1-bg" : "set-2-bg");

            skillSetBtnTextElement.text = setId == 1 ? "LB" : "RB";

            UpdateDataDrivenContent();
        }

        public void OnUpdate(PanelForSkillsSkillSetCompData dataIn)
        {
            if (!data.IsTheSameAs(dataIn))
            {
                data = dataIn;
                UpdateDataDrivenContent();
            }
        }

        private void UpdateDataDrivenContent()
        {
            if (data.isOn) skillSetElement.AddToClassList("panel-border-is-on");

            UpdateSlot(skillSetSlot1Element, data.slot1);
            UpdateSlot(skillSetSlot2Element, data.slot2);
            UpdateSlot(skillSetSlot3Element, data.slot3);
            UpdateSlot(skillSetSlot4Element, data.slot4);
        }

        private static void UpdateSlot(VisualElement slotElement, ElementalType slot)
        {
            ElementalType.All().ForEach(t => slotElement.RemoveFromClassList(t.bgColorCssClass));
            slotElement.AddToClassList(slot.bgColorCssClass);
        }
    }

    public class PanelForSkillsSkillSetCompData
    {
        public bool isOn = false;

        public ElementalType slot1 = ElementalType.NOT_SET;
        public ElementalType slot2 = ElementalType.NOT_SET;
        public ElementalType slot3 = ElementalType.NOT_SET;
        public ElementalType slot4 = ElementalType.NOT_SET;

        public bool IsTheSameAs(PanelForSkillsSkillSetCompData dataIn)
        {
            return isOn == dataIn.isOn &&
                   slot1 == dataIn.slot1 &&
                   slot2 == dataIn.slot2 &&
                   slot3 == dataIn.slot3 &&
                   slot4 == dataIn.slot4;
        }
    }
}