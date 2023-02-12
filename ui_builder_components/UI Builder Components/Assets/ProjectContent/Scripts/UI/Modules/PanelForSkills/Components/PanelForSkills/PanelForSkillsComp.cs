using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Base.Comp;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__Skill;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__SkillSet;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills
{
    /// <summary>
    /// CS representation of the component, turns all changeable content to variables 
    /// </summary>
    public class PanelForSkillsComp : IComp<PanelForSkillsCompDataBundle>
    {
        private VisualElement root;
        private PanelForSkillsCompEls els; // only include content controlled directly by this root panel level, child components are handled by individual comps
        private PanelForSkillsCompDataBundle data; // root level data, contains data of all the child comps

        private PanelForSkillsSkillSetComp skillSet1Comp;
        private PanelForSkillsSkillSetComp skillSet2Comp;
        
        private PanelForSkillsSkillComp skill1Comp;

        public static PanelForSkillsComp Create(VisualElement root)
        {
            PanelForSkillsCompEls els = PanelForSkillsCompEls.Create(root);
            PanelForSkillsSkillSetComp skillSet1Comp = PanelForSkillsSkillSetComp.Create(els.skillSet1CompRoot, 1);
            PanelForSkillsSkillSetComp skillSet2Comp = PanelForSkillsSkillSetComp.Create(els.skillSet2CompRoot, 2);

            PanelForSkillsSkillComp skill1Comp = PanelForSkillsSkillComp.Create(els.skill1CompRoot, Skill.FIRE_BALL);

            return new()
            {
                root = root,
                els = els,

                skillSet1Comp = skillSet1Comp,
                skillSet2Comp = skillSet2Comp,

                skill1Comp = skill1Comp,
            };
        }

        public void Init(PanelForSkillsCompDataBundle dataIn)
        {
            data = dataIn;
            els.InitStaticEls(data);
            els.UpdateDataDrivenEls(data);

            skillSet1Comp.Init(data.skillSet1CompData);
            skillSet2Comp.Init(data.skillSet2CompData);

            skill1Comp.Init(data.skill1CompData);
        }

        public void OnUpdate(PanelForSkillsCompDataBundle dataIn)
        {
            if (!data.IsTheSameAs(dataIn))
            {
                data = dataIn;
                els.UpdateDataDrivenEls(data);

                skillSet1Comp.OnUpdate(data.skillSet1CompData);
                skillSet2Comp.OnUpdate(data.skillSet2CompData);

                skill1Comp.OnUpdate(data.skill1CompData);
            }
        }
    }

    public class PanelForSkillsCompEls : ICompEls<PanelForSkillsCompDataBundle>
    {
        public VisualElement skillSet1CompRoot;
        public VisualElement skillSet2CompRoot;

        public VisualElement skill1CompRoot;

        private Label title;

        public static PanelForSkillsCompEls Create(VisualElement compRoot)
        {
            Label title = compRoot.Q<Label>("label__component-title");
            VisualElement skillSet1CompRoot = compRoot.Q<VisualElement>("i__panel-for-skills__skill-set__set-1");
            VisualElement skillSet2CompRoot = compRoot.Q<VisualElement>("i__panel-for-skills__skill-set__set-2");

            VisualElement skill1CompRoot = compRoot.Q<VisualElement>("i__panel-for-skills__skill-1");

            return new()
            {
                // static
                title = title,

                // comps - create them, but only holds a ref, comps handle themselves
                skillSet1CompRoot = skillSet1CompRoot,
                skillSet2CompRoot = skillSet2CompRoot,

                skill1CompRoot = skill1CompRoot,
            };
        }

        public void InitStaticEls(PanelForSkillsCompDataBundle dataIn)
        {
            title.text = "SKILL2";
        }

        public void UpdateDataDrivenEls(PanelForSkillsCompDataBundle dataIn)
        {
        }
    }

    public class PanelForSkillsCompDataBundle : ICompData<PanelForSkillsCompDataBundle>
    {
        public PanelForSkillsSkillSetCompData skillSet1CompData;
        public PanelForSkillsSkillSetCompData skillSet2CompData;
        public PanelForSkillsSkillCompData skill1CompData;

        public bool IsTheSameAs(PanelForSkillsCompDataBundle dataIn)
        {
            return skillSet1CompData.IsTheSameAs(dataIn.skillSet1CompData) &&
                   skillSet2CompData.IsTheSameAs(dataIn.skillSet2CompData) &&
                   skill1CompData.IsTheSameAs(dataIn.skill1CompData);
        }
    }
}