using System.Collections.Generic;
using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Base.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.EquipSkill.Components
{
    public class SkillEquipPanelSkillCompController : MonoBehaviour
    {
        private VisualElement root;
        private VisualElement skillEl;
        private VisualElement skillSelectionCursorEl;

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            skillEl = root.Q<VisualElement>("SkillEquipPanelSkill");
            skillSelectionCursorEl = root.Q<VisualElement>("SkillSelectionCursor");

            UiUtil.ToggleClass(skillSelectionCursorEl, "hidden", false);
        }

        private void AddFileContentToDocument()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("./skill_equip_panel.uxml").CloneTree();
            root.Add(ui);
        }
    }
}