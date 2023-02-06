using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Base.Util;
using UnityEditor;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__Skill
{
    public class PanelForSkillsSkillMono
    {
        public VisualElement root;

        public static PanelForSkillsSkillMono Create(VisualElement rootIn, Skill skill, Skill selectedSkill, bool isSkillActive)
        {
            VisualElement root = rootIn;
            // VisualElement root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("project://database/Assets/ProjectContent/Scripts/UI/Modules/PanelForSkills/Components/SkillEquipPanelSkillComp.uxml").CloneTree();
            VisualElement skillEl = root.Q<VisualElement>("ve__panel-for-skill__skill");
            UiUtil.ToggleClass(skillEl, "is-active-box", isSkillActive);

            Label skillSlotCountEl = root.Q<Label>("label__panel-for-skill__skill-slot-cost-label");
            skillSlotCountEl.text = "" + skill.slots;

            VisualElement skillSelectionCursorEl = root.Q<VisualElement>("instance__box-selection-cursor");
            UiUtil.ToggleClass(skillSelectionCursorEl, "hidden", (skill != selectedSkill));

            return new PanelForSkillsSkillMono
            {
                root = root
            };
        }
    }
}