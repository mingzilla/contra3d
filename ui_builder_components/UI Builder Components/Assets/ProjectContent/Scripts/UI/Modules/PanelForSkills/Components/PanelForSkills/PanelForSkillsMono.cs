using System;
using BaseUtil.GameUtil.Base;
using ProjectContent.Scripts.Types;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.BaseStore.Domain;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__Skill;
using ProjectContent.Scripts.UI.Modules.PanelForSkills.Components.PanelForSkills__SkillSet;
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
        public PanelForSkillsCompDataBundle data;

        public static PanelForSkillsMono instance;

        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
        }

        private void Start()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            comp = PanelForSkillsComp.Create(root);
            Init();
        }

        private void Update()
        {
            comp.OnUpdate(data);
        }

        private void Init()
        {
            data = new()
            {
                skillSet1CompData = new PanelForSkillsSkillSetCompData() {isOn = true, slot1 = ElementalType.FIRE, slot2 = ElementalType.LIGHT},
                skillSet2CompData = new PanelForSkillsSkillSetCompData() {isOn = false, slot1 = ElementalType.WATER, slot2 = ElementalType.LIGHT, slot3 = ElementalType.WATER},
                skillRowColMatrix = new[]
                {
                    new[]
                    {
                        new PanelForSkillsSkillCompData() {set1Input = GameInputKey.A, isSelected = true},
                        new PanelForSkillsSkillCompData() {set1Input = GameInputKey.B},
                        new PanelForSkillsSkillCompData() { },
                        new PanelForSkillsSkillCompData() {set1Input = GameInputKey.X},
                        new PanelForSkillsSkillCompData() {set2Input = GameInputKey.A},
                    },
                }
            };

            comp.Init(data);
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