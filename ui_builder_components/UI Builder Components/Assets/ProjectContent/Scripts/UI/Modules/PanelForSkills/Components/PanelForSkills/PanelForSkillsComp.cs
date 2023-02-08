using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Base.Comp;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__SkillSet;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills
{
    /// <summary>
    /// CS representation of the component, turns all changeable content to variables 
    /// </summary>
    public class PanelForSkillsComp : IComp<PanelForSkillsCompData>
    {
        private VisualElement root;
        private PanelForSkillsCompEls els; // only include content controlled directly by this root panel level, child components are handled by individual comps
        private PanelForSkillsCompData data;

        private PanelForSkillsSkillSetComp skillSet1Comp;

        public static PanelForSkillsComp Create(VisualElement root)
        {
            PanelForSkillsCompEls els = PanelForSkillsCompEls.Create(root);
            PanelForSkillsSkillSetComp skillSet1Comp = PanelForSkillsSkillSetComp.Create(els.skillSet1CompRoot, 1);

            return new()
            {
                root = root,
                els = els,
                skillSet1Comp = skillSet1Comp,
            };
        }

        public void Init(PanelForSkillsCompData dataIn)
        {
            data = dataIn;
            els.InitStaticEls(dataIn);
            els.UpdateDynamicEls(data);

            skillSet1Comp.Init(dataIn.skillSet1CompData);
        }

        public void OnUpdate(PanelForSkillsCompData dataIn)
        {
            if (!data.IsTheSameAs(dataIn))
            {
                data = dataIn;
                els.UpdateDynamicEls(data);

                skillSet1Comp.OnUpdate(dataIn.skillSet1CompData);
            }
        }
    }

    public class PanelForSkillsCompEls : ICompEls<PanelForSkillsCompData>
    {
        public VisualElement skillSet1CompRoot;
        private Label title;

        public static PanelForSkillsCompEls Create(VisualElement compRoot)
        {
            Label title = compRoot.Q<Label>("label__component-title");
            VisualElement skillSet1CompRoot = compRoot.Q<VisualElement>("panel-for-skills__skill-set__set-1");

            return new()
            {
                // static
                title = title,

                // comps
                skillSet1CompRoot = skillSet1CompRoot,
            };
        }

        public void InitStaticEls(PanelForSkillsCompData dataIn)
        {
            title.text = "SKILL2";
        }

        public void UpdateDynamicEls(PanelForSkillsCompData dataIn)
        {
        }
    }

    public class PanelForSkillsCompData : ICompData<PanelForSkillsCompData>
    {
        public PanelForSkillsSkillSetCompData skillSet1CompData;

        public bool IsTheSameAs(PanelForSkillsCompData dataIn)
        {
            return skillSet1CompData.IsTheSameAs(dataIn.skillSet1CompData);
        }
    }
}