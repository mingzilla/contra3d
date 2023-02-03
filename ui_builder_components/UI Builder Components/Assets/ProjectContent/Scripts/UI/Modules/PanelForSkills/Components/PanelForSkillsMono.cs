using System.Collections.Generic;
using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.BaseStore.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components
{
    public class PanelForSkillsMono : MonoBehaviour
    {
        private VisualElement root;
        private VisualElement skillsTable;
        public int relatedPlayerId = 1;

        private PanelForSkillsStoreData storeData = new();

        [SerializeField] private GameObject skillEquipPanelSkillCompPrefab;

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
                VisualElement skillRowTypeImage = skillRow.Q<VisualElement>("PanelForSkillsSkillTypeCompImage");
                VisualElement skillRowContent = skillRow.Q<VisualElement>("SkillEquipPanelSkillRowContent");

                ElementalType type = elementalTypeMap[(skillRow.name)];
                skillRowTypeImage.AddToClassList(type.imageCssClass);

                List<Skill> skills = skillGroup[(skillRow.name)];
                skills.ForEach(skill =>
                {
                    VisualElement skillEquipPanelSkillComp = Instantiate(skillEquipPanelSkillCompPrefab, transform.position, Quaternion.identity).GetComponent<UIDocument>().rootVisualElement;
                    bool isSkillActive = storeData.activeItems.isSkillActive(skill);
                    PanelForSkillsSkillMono panelForSkillsSkillBox = PanelForSkillsSkillMono.Create(skillEquipPanelSkillComp, skill, selectedSkill, isSkillActive);
                    skillRowContent.Add(panelForSkillsSkillBox.root);
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