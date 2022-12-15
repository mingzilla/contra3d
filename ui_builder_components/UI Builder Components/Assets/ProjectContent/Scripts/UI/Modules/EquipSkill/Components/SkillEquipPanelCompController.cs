using System.Collections.Generic;
using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Modules.EquipSkill.BaseStore.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.EquipSkill.Components
{
    public class SkillEquipPanelCompController : MonoBehaviour
    {
        private VisualElement root;
        private VisualElement skillsTable;
        public int relatedPlayerId = 1;

        private EquipSkillStoreData storeData = new();

        private void OnEnable()
        {
            Skill selectedSkill = storeData.activeItems.selectedSkill;
            root = GetComponent<UIDocument>().rootVisualElement;
            skillsTable = root.Q<VisualElement>("SkillEquipPanelSkillsTable");
            VisualElement[] skillRows =
            {
                skillsTable.Q<VisualElement>("NEUTRAL"),
                skillsTable.Q<VisualElement>("FIRE"),
                skillsTable.Q<VisualElement>("WATER"),
                skillsTable.Q<VisualElement>("LIGHT"),
            };

            Dictionary<string, ElementalType> elementalTypeMap = ElementalType.ItemsMap();
            Dictionary<string, List<Skill>> skillGroup = Skill.AllGroupByStringType();

            foreach (VisualElement skillRow in skillRows)
            {
                VisualElement skillRowTypeImage = skillRow.Q<VisualElement>("SkillEquipPanelSkillTypeImage");
                VisualElement skillRowContent = skillRow.Q<VisualElement>("SkillEquipPanelSkillRowContent");

                ElementalType type = elementalTypeMap[(skillRow.name)];
                skillRowTypeImage.AddToClassList(type.imageCssClass);
                
                List<Skill> skills = skillGroup[(skillRow.name)];
                skills.ForEach(skill =>
                {
                    bool isSkillActive = storeData.activeItems.isSkillActive(skill);
                    SkillEquipPanelSkillComp skillBox = SkillEquipPanelSkillComp.Create(skill, selectedSkill, isSkillActive);
                    skillRowContent.Add(skillBox.root);
                });
            }
        }

        private void AddEvent()
        {
            Button button = new()
            {
                name = "Hi",
                text = "Hi",
            };
            button.clicked += () =>
            {
                Debug.Log(button.name);
            };
        }
    }
}