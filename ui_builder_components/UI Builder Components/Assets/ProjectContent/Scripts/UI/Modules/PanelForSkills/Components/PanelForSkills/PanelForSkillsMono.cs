using ProjectContent.Scripts.UI.Modules.PanelForSkills.BaseStore.Domain;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills
{
    public class PanelForSkillsMono : MonoBehaviour
    {
        private VisualElement root;
        private VisualElement skillsTable;
        public int relatedPlayerId = 1;

        private PanelForSkillsStoreData storeData = new();

        [SerializeField] private GameObject skillEquipPanelSkillCompPrefab;

        private PanelForSkillsComp comp;

        private void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            comp = PanelForSkillsComp.Create(root);
            comp.BuildOnStart();
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