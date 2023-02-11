using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Base.Comp;
using ProjectContent.Scripts.UI.Base.Util;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__Skill
{
    public class PanelForSkillsSkillComp : IComp<PanelForSkillsSkillCompData>
    {
        private VisualElement compRoot; // root of current component

        private PanelForSkillsSkillCompEls els;
        private PanelForSkillsSkillCompData data;

        public static PanelForSkillsSkillComp Create(VisualElement compRoot, Skill skill)
        {
            return new()
            {
                compRoot = compRoot,
                els = PanelForSkillsSkillCompEls.Create(compRoot, skill),
            };
        }

        public void Init(PanelForSkillsSkillCompData dataIn)
        {
            data = dataIn;
            els.InitStaticEls(data);
            els.UpdateDataDrivenEls(data);
        }

        public void OnUpdate(PanelForSkillsSkillCompData dataIn)
        {
            if (!data.IsTheSameAs(dataIn))
            {
                data = dataIn;
                els.UpdateDataDrivenEls(data);
            }
        }
    }

    public class PanelForSkillsSkillCompEls : ICompEls<PanelForSkillsSkillCompData>
    {
        public Skill skill;

        // static
        private VisualElement icon;

        // data driven
        private VisualElement border;

        private VisualElement skillSet1;
        private Label skillSet1BtnText;

        private VisualElement skillSet2;
        private Label skillSet2BtnText;

        public static PanelForSkillsSkillCompEls Create(VisualElement compRoot, Skill skill)
        {
            VisualElement icon = compRoot.Q<VisualElement>("ve__panel-for-skills__skill__icon");
            VisualElement border = compRoot.Q<VisualElement>("ve__panel-for-skills__skill__border");

            VisualElement skillSet1 = compRoot.Q<VisualElement>("ve__panel-for-skills__skill__active-set-1");
            Label skillSet1BtnText = compRoot.Q<Label>("ve__panel-for-skills__skill__active-set-1__button-text");

            VisualElement skillSet2 = compRoot.Q<VisualElement>("ve__panel-for-skills__skill__active-set-2");
            Label skillSet2BtnText = compRoot.Q<Label>("ve__panel-for-skills__skill__active-set-2__button-text");

            return new()
            {
                skill = skill,

                // static
                icon = icon,

                // data driven
                border = border,

                skillSet1 = skillSet1,
                skillSet1BtnText = skillSet1BtnText,

                skillSet2 = skillSet2,
                skillSet2BtnText = skillSet2BtnText,
            };
        }

        public void InitStaticEls(PanelForSkillsSkillCompData data)
        {
            icon.AddToClassList(skill.iconCss);
        }

        public void UpdateDataDrivenEls(PanelForSkillsSkillCompData data)
        {
            if (data.isActive) border.AddToClassList("panel-with-border panel-border-is-selected");
            if (!data.isActive) border.RemoveFromClassList("panel-with-border");
            if (!data.isActive) border.RemoveFromClassList("panel-border-is-selected");

            UiUtil.ToggleClass(skillSet1, "is-hidden", data.set1Input == null);
            if (data.set1Input != null) skillSet1BtnText.text = data.set1Input.name;

            UiUtil.ToggleClass(skillSet2, "is-hidden", data.set2Input == null);
            if (data.set2Input != null) skillSet2BtnText.text = data.set2Input.name;
        }
    }

    public class PanelForSkillsSkillCompData : ICompData<PanelForSkillsSkillCompData>
    {
        public bool isActive = false; // currently the cursor is on it
        public GameInputKey set1Input = null; // null, A, B, X, Y
        public GameInputKey set2Input = null; // null, A, B, X, Y

        public bool IsTheSameAs(PanelForSkillsSkillCompData dataIn)
        {
            return isActive == dataIn.isActive &&
                   set1Input == dataIn.set1Input &&
                   set2Input == dataIn.set2Input;
        }
    }
}