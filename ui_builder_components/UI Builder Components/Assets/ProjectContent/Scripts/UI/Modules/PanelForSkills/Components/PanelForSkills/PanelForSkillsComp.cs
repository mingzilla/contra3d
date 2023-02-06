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

        public static PanelForSkillsComp Create(VisualElement root)
        {
            Label titleElement = root.Q<Label>("label__component-title");
            return new()
            {
                root = root,
                titleElement = titleElement,
            };
        }

        public void BuildOnStart()
        {
            titleElement.text = title;
        }
    }
}