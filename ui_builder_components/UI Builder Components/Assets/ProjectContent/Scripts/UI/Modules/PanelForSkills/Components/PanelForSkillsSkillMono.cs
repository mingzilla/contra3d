using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Base.Util;
using UnityEditor;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components
{
    public class PanelForSkillsSkillMono
    {
        public VisualElement root;

        public static PanelForSkillsSkillMono Create(VisualElement rootIn, Skill skill, Skill selectedSkill, bool isSkillActive)
        {
            VisualElement root = rootIn;
            // VisualElement root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/ProjectContent/Scripts/UI/Modules/PanelForSkills/Components/SkillEquipPanelSkillComp.uxml").CloneTree();
            VisualElement skillEl = root.Q<VisualElement>("SkillEquipPanelSkill");
            UiUtil.ToggleClass(skillEl, "is-active-box", isSkillActive);

            Label skillSlotCountEl = root.Q<Label>("SkillSlotCount");
            skillSlotCountEl.text = "" + skill.slots;

            VisualElement skillSelectionCursorEl = root.Q<VisualElement>("SkillSelectionCursor");
            UiUtil.ToggleClass(skillSelectionCursorEl, "hidden", (skill != selectedSkill));

            return new PanelForSkillsSkillMono
            {
                root = root
            };
        }
    }
}