﻿using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Base.Comp;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__SkillSet
{
    public class PanelForSkillsSkillSetComp : IComp<PanelForSkillsSkillSetCompData>
    {
        private VisualElement compRoot; // root of current component
        private int setId;

        private PanelForSkillsSkillSetCompEls els;
        private PanelForSkillsSkillSetCompData data;

        public static PanelForSkillsSkillSetComp Create(VisualElement compRoot, int setId)
        {
            return new()
            {
                compRoot = compRoot,
                setId = setId,
                els = PanelForSkillsSkillSetCompEls.Create(compRoot, setId),
            };
        }

        public void Init(PanelForSkillsSkillSetCompData dataIn)
        {
            data = dataIn;
            els.InitStaticEls(data);
            els.UpdateDataDrivenEls(data);
        }

        public void OnUpdate(PanelForSkillsSkillSetCompData dataIn)
        {
            if (!data.IsTheSameAs(dataIn))
            {
                data = dataIn;
                els.UpdateDataDrivenEls(data);
            }
        }
    }

    public class PanelForSkillsSkillSetCompEls : ICompEls<PanelForSkillsSkillSetCompData>
    {
        private int setId;

        private VisualElement skillSetStaff;
        private VisualElement skillSetBtnIndicator;
        private Label skillSetBtnText;

        private VisualElement skillSet;
        private VisualElement skillSetSlot1;
        private VisualElement skillSetSlot2;
        private VisualElement skillSetSlot3;
        private VisualElement skillSetSlot4;

        public static PanelForSkillsSkillSetCompEls Create(VisualElement compRoot, int setId)
        {
            VisualElement skillSetStaff = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__staff");
            VisualElement skillSetBtnIndicator = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__btn-indicator");
            Label skillSetBtnText = compRoot.Q<Label>("ve__panel-for-skills__skill-set__set__btn-text");

            VisualElement skillSet = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set");
            VisualElement skillSetSlot1 = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__slot-1");
            VisualElement skillSetSlot2 = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__slot-2");
            VisualElement skillSetSlot3 = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__slot-3");
            VisualElement skillSetSlot4 = compRoot.Q<VisualElement>("ve__panel-for-skills__skill-set__set__slot-4");

            return new()
            {
                setId = setId,

                // static
                skillSetStaff = skillSetStaff,
                skillSetBtnIndicator = skillSetBtnIndicator,
                skillSetBtnText = skillSetBtnText,

                // data driven
                skillSet = skillSet,
                skillSetSlot1 = skillSetSlot1,
                skillSetSlot2 = skillSetSlot2,
                skillSetSlot3 = skillSetSlot3,
                skillSetSlot4 = skillSetSlot4,
            };
        }

        public void InitStaticEls(PanelForSkillsSkillSetCompData data)
        {
            skillSetStaff.RemoveFromClassList("staff-equipment-1");
            skillSetStaff.AddToClassList(setId == 1 ? "staff-equipment-1" : "staff-equipment-2");

            skillSetBtnIndicator.RemoveFromClassList("set-1-bg");
            skillSetBtnIndicator.AddToClassList(setId == 1 ? "set-1-bg" : "set-2-bg");

            skillSetBtnText.text = setId == 1 ? "LB" : "RB";
        }

        public void UpdateDataDrivenEls(PanelForSkillsSkillSetCompData data)
        {
            if (data.isOn) skillSet.AddToClassList("panel-border-is-on");
            if (!data.isOn) skillSet.RemoveFromClassList("panel-border-is-on");
            UpdateSlot(skillSetSlot1, data.slot1);
            UpdateSlot(skillSetSlot2, data.slot2);
            UpdateSlot(skillSetSlot3, data.slot3);
            UpdateSlot(skillSetSlot4, data.slot4);
        }

        private static void UpdateSlot(VisualElement slotElement, ElementalType slot)
        {
            ElementalType.All().ForEach(t => slotElement.RemoveFromClassList(t.bgColorCssClass));
            slotElement.AddToClassList(slot.bgColorCssClass);
        }
    }

    public class PanelForSkillsSkillSetCompData : AbstractCompData<PanelForSkillsSkillSetCompData>
    {
        public bool isOn = false;

        public ElementalType slot1 = ElementalType.NOT_SET;
        public ElementalType slot2 = ElementalType.NOT_SET;
        public ElementalType slot3 = ElementalType.NOT_SET;
        public ElementalType slot4 = ElementalType.NOT_SET;
    }
}