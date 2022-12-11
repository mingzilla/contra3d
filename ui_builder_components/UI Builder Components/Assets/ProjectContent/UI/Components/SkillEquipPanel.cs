using System.Collections.Generic;
using ProjectContent.Scripts.Types;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectContent.UI.Components
{
    public class SkillEquipPanel : MonoBehaviour
    {
        private VisualElement root;

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            VisualElement skillEquipPanel = root.Q<VisualElement>("SkillEquipPanel");

            Dictionary<ElementalType, List<Skill>> skillGroup = Skill.AllGroupByType();

            ElementalType.All().ForEach(type =>
            {
                List<Skill> skills = skillGroup[(type)];
                skills.ForEach(skill =>
                {
                    Button button = new();
                    button.name = skill.name;
                    button.text = skill.name;

                    button.clicked += () =>
                    {
                        Debug.Log(button.name);
                    };
                    
                    skillEquipPanel.Add(button);                    
                });
            });
        }

        private void AddFileContentToDocument()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement ui = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("./skill_equip_panel.uxml").CloneTree();
            root.Add(ui);
        }
    }
}