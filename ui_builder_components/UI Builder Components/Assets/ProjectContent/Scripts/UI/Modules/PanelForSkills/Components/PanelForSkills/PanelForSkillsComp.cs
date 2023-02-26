using BaseUtil.Base;
using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Base.Comp;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__Skill;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__SkillSet;
using UnityEngine;
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

        private PanelForSkillsSkillComp skillCompRow1Col1;
        private PanelForSkillsSkillComp skillCompRow1Col2;
        private PanelForSkillsSkillComp skillCompRow1Col3;
        private PanelForSkillsSkillComp skillCompRow1Col4;
        private PanelForSkillsSkillComp skillCompRow1Col5;

        public static PanelForSkillsComp Create(VisualElement root)
        {
            PanelForSkillsCompEls els = PanelForSkillsCompEls.Create(root);
            PanelForSkillsSkillSetComp skillSet1Comp = PanelForSkillsSkillSetComp.Create(els.skillSet1CompRoot, 1);
            PanelForSkillsSkillSetComp skillSet2Comp = PanelForSkillsSkillSetComp.Create(els.skillSet2CompRoot, 2);

            PanelForSkillsSkillComp skillCompRow1Col1 = PanelForSkillsSkillComp.Create(els.skillCompRootRow1Col1, Skill.FIRE_BALL);
            PanelForSkillsSkillComp skillCompRow1Col2 = PanelForSkillsSkillComp.Create(els.skillCompRootRow1Col2, Skill.FIRE_BOUNCE_BALL);
            PanelForSkillsSkillComp skillCompRow1Col3 = PanelForSkillsSkillComp.Create(els.skillCompRootRow1Col3, Skill.BIG_FIRE_BALL);
            PanelForSkillsSkillComp skillCompRow1Col4 = PanelForSkillsSkillComp.Create(els.skillCompRootRow1Col4, Skill.METEOR_STORM);
            PanelForSkillsSkillComp skillCompRow1Col5 = PanelForSkillsSkillComp.Create(els.skillCompRootRow1Col5, Skill.FIRE_SPRAY);

            return new()
            {
                root = root,
                els = els,

                skillSet1Comp = skillSet1Comp,
                skillSet2Comp = skillSet2Comp,

                skillCompRow1Col1 = skillCompRow1Col1,
                skillCompRow1Col2 = skillCompRow1Col2,
                skillCompRow1Col3 = skillCompRow1Col3,
                skillCompRow1Col4 = skillCompRow1Col4,
                skillCompRow1Col5 = skillCompRow1Col5,
            };
        }

        public void Init(PanelForSkillsCompDataBundle dataIn)
        {
            data = dataIn;
            els.InitStaticEls(data);
            els.UpdateDataDrivenEls(data);

            skillSet1Comp.Init(data.skillSet1CompData);
            skillSet2Comp.Init(data.skillSet2CompData);

            skillCompRow1Col1.Init(data.skillCompDataRow1Col1);
            skillCompRow1Col2.Init(data.skillCompDataRow1Col2);
            skillCompRow1Col3.Init(data.skillCompDataRow1Col3);
            skillCompRow1Col4.Init(data.skillCompDataRow1Col4);
            skillCompRow1Col5.Init(data.skillCompDataRow1Col5);
        }

        public void OnUpdate(PanelForSkillsCompDataBundle dataIn)
        {
            if (!data.IsTheSameAs(dataIn))
            {
                data = dataIn;
                els.UpdateDataDrivenEls(data);

                skillSet1Comp.OnUpdate(data.skillSet1CompData);
                skillSet2Comp.OnUpdate(data.skillSet2CompData);

                skillCompRow1Col1.OnUpdate(data.skillCompDataRow1Col1);
                skillCompRow1Col2.OnUpdate(data.skillCompDataRow1Col2);
                skillCompRow1Col3.OnUpdate(data.skillCompDataRow1Col3);
                skillCompRow1Col4.OnUpdate(data.skillCompDataRow1Col4);
                skillCompRow1Col5.OnUpdate(data.skillCompDataRow1Col5);
            }
        }
    }

    public class PanelForSkillsCompEls : ICompEls<PanelForSkillsCompDataBundle>
    {
        public VisualElement skillSet1CompRoot;
        public VisualElement skillSet2CompRoot;

        public VisualElement skillCompRootRow1Col1;
        public VisualElement skillCompRootRow1Col2;
        public VisualElement skillCompRootRow1Col3;
        public VisualElement skillCompRootRow1Col4;
        public VisualElement skillCompRootRow1Col5;

        private Label title;

        public static PanelForSkillsCompEls Create(VisualElement compRoot)
        {
            Label title = compRoot.Q<Label>("label__component-title");
            VisualElement skillSet1CompRoot = compRoot.Q<VisualElement>("i__panel-for-skills__skill-set__set-1");
            VisualElement skillSet2CompRoot = compRoot.Q<VisualElement>("i__panel-for-skills__skill-set__set-2");

            VisualElement skillCompRootRow1Col1 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-1");
            VisualElement skillCompRootRow1Col2 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-2");
            VisualElement skillCompRootRow1Col3 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-3");
            VisualElement skillCompRootRow1Col4 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-4");
            VisualElement skillCompRootRow1Col5 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-5");

            return new()
            {
                // static
                title = title,

                // comps - create them, but only holds a ref, comps handle themselves
                skillSet1CompRoot = skillSet1CompRoot,
                skillSet2CompRoot = skillSet2CompRoot,

                skillCompRootRow1Col1 = skillCompRootRow1Col1,
                skillCompRootRow1Col2 = skillCompRootRow1Col2,
                skillCompRootRow1Col3 = skillCompRootRow1Col3,
                skillCompRootRow1Col4 = skillCompRootRow1Col4,
                skillCompRootRow1Col5 = skillCompRootRow1Col5,
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

    /// <summary>
    /// Comp Data Bundle is used at top level, which directly includes all the lowest level Comp Data objects. There will be no nested Comp Data definition.
    /// Comp Data are supposed to be flat data structures, which don't include other Comp Data 
    /// </summary>
    public class PanelForSkillsCompDataBundle : AbstractCompData<PanelForSkillsCompDataBundle>
    {
        public PanelForSkillsSkillSetCompData skillSet1CompData;
        public PanelForSkillsSkillSetCompData skillSet2CompData;
        public PanelForSkillsSkillCompData skillCompDataRow1Col1;
        public PanelForSkillsSkillCompData skillCompDataRow1Col2;
        public PanelForSkillsSkillCompData skillCompDataRow1Col3;
        public PanelForSkillsSkillCompData skillCompDataRow1Col4;
        public PanelForSkillsSkillCompData skillCompDataRow1Col5;

        public PanelForSkillsCompDataBundle UpdateSkillSet(bool left, bool right)
        {
            PanelForSkillsCompDataBundle bundle = this;
            bundle = Fn.SetIn(bundle, new[] {"skillSet1CompData", "isOn"}, left);
            bundle = Fn.SetIn(bundle, new[] {"skillSet2CompData", "isOn"}, right);
            return bundle;
        }
    }
}