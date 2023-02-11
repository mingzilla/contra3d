using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Base.Util;
using UnityEditor;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__Skill_BK
{
    public class PanelForSkillsSkillMonoBK
    {
        public VisualElement root;

        public static PanelForSkillsSkillMonoBK Create(VisualElement rootIn, Skill skill, Skill selectedSkill, bool isSkillActive)
        {
            VisualElement root = rootIn;
            // VisualElement root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("project://database/Assets/ProjectContent/Scripts/UI/Modules/PanelForSkills/Components/SkillEquipPanelSkillComp.uxml").CloneTree();
            VisualElement skillEl = root.Q<VisualElement>("ve__panel-for-skill__skill");
            UiUtil.ToggleClass(skillEl, "is-active-box", isSkillActive);

            VisualElement skillSelectionCursorEl = root.Q<VisualElement>("instance__box-selection-cursor");
            UiUtil.ToggleClass(skillSelectionCursorEl, "hidden", (skill != selectedSkill));

            return new PanelForSkillsSkillMonoBK
            {
                root = root
            };
        }
    }
}