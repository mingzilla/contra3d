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
        public int relatedPlayerId = 1;

        private EquipSkillStoreData storeData = new();

        private void OnEnable()
        {
            Skill selectedSkill = storeData.activeItems.selectedSkill;
            root = GetComponent<UIDocument>().rootVisualElement;
            VisualElement[] skillContainers =
            {
                root.Q<VisualElement>("NEUTRAL"),
                root.Q<VisualElement>("FIRE"),
                root.Q<VisualElement>("WATER"),
                root.Q<VisualElement>("LIGHT"),
            };

            Dictionary<string, List<Skill>> skillGroup = Skill.AllGroupByStringType();

            foreach (VisualElement skillContainer in skillContainers)
            {
                List<Skill> skills = skillGroup[(skillContainer.name)];
                skills.ForEach(skill =>
                {
                    bool isSkillActive = storeData.activeItems.isSkillActive(skill);
                    SkillEquipPanelSkillComp skillBox = SkillEquipPanelSkillComp.Create(skill, selectedSkill, isSkillActive);
                    skillContainer.Add(skillBox.root);
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