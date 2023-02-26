using BaseUtil.Base;
using ProjectContent.Scripts.Player.PanelSelection;
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

        private PanelForSkillsSkillComp skillCompRow0Col0;
        private PanelForSkillsSkillComp skillCompRow0Col1;
        private PanelForSkillsSkillComp skillCompRow0Col2;
        private PanelForSkillsSkillComp skillCompRow0Col3;
        private PanelForSkillsSkillComp skillCompRow0Col4;

        public static PanelForSkillsComp Create(VisualElement root)
        {
            PanelForSkillsCompEls els = PanelForSkillsCompEls.Create(root);
            PanelForSkillsSkillSetComp skillSet1Comp = PanelForSkillsSkillSetComp.Create(els.skillSet1CompRoot, 1);
            PanelForSkillsSkillSetComp skillSet2Comp = PanelForSkillsSkillSetComp.Create(els.skillSet2CompRoot, 2);

            PanelForSkillsSkillComp skillCompRow0Col0 = PanelForSkillsSkillComp.Create(els.skillCompRootRow0Col0, Skill.FIRE_BALL);
            PanelForSkillsSkillComp skillCompRow0Col1 = PanelForSkillsSkillComp.Create(els.skillCompRootRow0Col1, Skill.FIRE_BOUNCE_BALL);
            PanelForSkillsSkillComp skillCompRow0Col2 = PanelForSkillsSkillComp.Create(els.skillCompRootRow0Col2, Skill.BIG_FIRE_BALL);
            PanelForSkillsSkillComp skillCompRow0Col3 = PanelForSkillsSkillComp.Create(els.skillCompRootRow0Col3, Skill.METEOR_STORM);
            PanelForSkillsSkillComp skillCompRow0Col4 = PanelForSkillsSkillComp.Create(els.skillCompRootRow0Col4, Skill.FIRE_SPRAY);

            return new()
            {
                root = root,
                els = els,

                skillSet1Comp = skillSet1Comp,
                skillSet2Comp = skillSet2Comp,

                skillCompRow0Col0 = skillCompRow0Col0,
                skillCompRow0Col1 = skillCompRow0Col1,
                skillCompRow0Col2 = skillCompRow0Col2,
                skillCompRow0Col3 = skillCompRow0Col3,
                skillCompRow0Col4 = skillCompRow0Col4,
            };
        }

        public void Init(PanelForSkillsCompDataBundle dataIn)
        {
            data = dataIn;
            els.InitStaticEls(data);
            els.UpdateDataDrivenEls(data);

            skillSet1Comp.Init(data.skillSet1CompData);
            skillSet2Comp.Init(data.skillSet2CompData);

            skillCompRow0Col0.Init(data.skillRowColMatrix[0][0]);
            skillCompRow0Col1.Init(data.skillRowColMatrix[0][1]);
            skillCompRow0Col2.Init(data.skillRowColMatrix[0][2]);
            skillCompRow0Col3.Init(data.skillRowColMatrix[0][3]);
            skillCompRow0Col4.Init(data.skillRowColMatrix[0][4]);
        }

        public void OnUpdate(PanelForSkillsCompDataBundle dataIn)
        {
            if (!data.IsTheSameAs(dataIn))
            {
                data = dataIn;
                els.UpdateDataDrivenEls(data);

                skillSet1Comp.OnUpdate(data.skillSet1CompData);
                skillSet2Comp.OnUpdate(data.skillSet2CompData);

                skillCompRow0Col0.OnUpdate(data.skillRowColMatrix[0][0]);
                skillCompRow0Col1.OnUpdate(data.skillRowColMatrix[0][1]);
                skillCompRow0Col2.OnUpdate(data.skillRowColMatrix[0][2]);
                skillCompRow0Col3.OnUpdate(data.skillRowColMatrix[0][3]);
                skillCompRow0Col4.OnUpdate(data.skillRowColMatrix[0][4]);
            }
        }
    }

    public class PanelForSkillsCompEls : ICompEls<PanelForSkillsCompDataBundle>
    {
        public VisualElement skillSet1CompRoot;
        public VisualElement skillSet2CompRoot;

        public VisualElement skillCompRootRow0Col0;
        public VisualElement skillCompRootRow0Col1;
        public VisualElement skillCompRootRow0Col2;
        public VisualElement skillCompRootRow0Col3;
        public VisualElement skillCompRootRow0Col4;

        private Label title;

        public static PanelForSkillsCompEls Create(VisualElement compRoot)
        {
            Label title = compRoot.Q<Label>("label__component-title");
            VisualElement skillSet1CompRoot = compRoot.Q<VisualElement>("i__panel-for-skills__skill-set__set-1");
            VisualElement skillSet2CompRoot = compRoot.Q<VisualElement>("i__panel-for-skills__skill-set__set-2");

            VisualElement skillCompRootRow0Col0 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-0");
            VisualElement skillCompRootRow0Col1 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-1");
            VisualElement skillCompRootRow0Col2 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-2");
            VisualElement skillCompRootRow0Col3 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-3");
            VisualElement skillCompRootRow0Col4 = compRoot.Q<VisualElement>("i__panel-for-skills__skill__row-1-col-4");

            return new()
            {
                // static
                title = title,

                // comps - create them, but only holds a ref, comps handle themselves
                skillSet1CompRoot = skillSet1CompRoot,
                skillSet2CompRoot = skillSet2CompRoot,

                skillCompRootRow0Col0 = skillCompRootRow0Col0,
                skillCompRootRow0Col1 = skillCompRootRow0Col1,
                skillCompRootRow0Col2 = skillCompRootRow0Col2,
                skillCompRootRow0Col3 = skillCompRootRow0Col3,
                skillCompRootRow0Col4 = skillCompRootRow0Col4,
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

        public PanelForSkillsSkillCompData[][] skillRowColMatrix;

        public PanelForSkillsCompDataBundle UpdateSkillSet(bool left, bool right)
        {
            PanelForSkillsCompDataBundle bundle = this;
            bundle = Fn.SetIn(bundle, new[] {"skillSet1CompData", "isOn"}, left);
            bundle = Fn.SetIn(bundle, new[] {"skillSet2CompData", "isOn"}, right);
            return bundle;
        }

        public PanelForSkillsCompDataBundle UpdateSkillCursor(SkillPanelSelectionState selectionState)
        {
            PanelForSkillsSkillCompData[][] newMatrix = Fn.Create2DArray<PanelForSkillsSkillCompData>(skillRowColMatrix.Length, skillRowColMatrix[0].Length);
            for (int rowIndex = 0; rowIndex < skillRowColMatrix.Length; rowIndex++)
            {
                PanelForSkillsSkillCompData[] row = skillRowColMatrix[rowIndex];
                for (int colIndex = 0; colIndex < row.Length; colIndex++)
                {
                    PanelForSkillsSkillCompData item = skillRowColMatrix[rowIndex][colIndex];
                    bool isSelectedItem = selectionState.currentSelectedRowIndex == rowIndex && selectionState.currentSelectedColIndex == colIndex;
                    newMatrix[rowIndex][colIndex] = Fn.SetIn(item, new[] {"isSelected"}, isSelectedItem);
                }
            }
            PanelForSkillsCompDataBundle bundle = Fn.SetIn(this, new[] {"skillRowColMatrix"}, newMatrix);
            return bundle;
        }
    }
}