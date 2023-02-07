using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__SkillSet;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills
{
    /// <summary>
    /// CS representation of the component, turns all changeable content to variables 
    /// </summary>
    public class PanelForSkillsComp
    {
        private VisualElement root;
        private Label titleElement;

        public string title = "SKILL2";

        private PanelForSkillsSkillSetComp skillSetComp;

        public static PanelForSkillsComp Create(VisualElement root)
        {
            Label titleElement = root.Q<Label>("label__component-title");

            PanelForSkillsSkillSetComp skillSetComp = PanelForSkillsSkillSetComp.Create(root, "panel-for-skills__skill-set__set-1", 1);

            return new()
            {
                root = root,
                titleElement = titleElement,
                skillSetComp = skillSetComp,
            };
        }

        public void BuildOnStart()
        {
            titleElement.text = title;
            skillSetComp.Init(new PanelForSkillsSkillSetCompData() {isOn = true, slot1 = ElementalType.FIRE, slot2 = ElementalType.LIGHT});
        }
    }
}