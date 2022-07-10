using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Types
{
    public class GameScene
    {
        public static readonly GameScene TITLE_SCREEN = Create("ContraTitle", false, GameControlState.TITLE_SCREEN_MENU, false, "");
        public static readonly GameScene LEVEL_1 = Create("ContraLv1", true, GameControlState.INFO_SCREEN, false, "Area 1");
        public static readonly GameScene LEVEL_2 = Create("ContraLv2", true, GameControlState.INFO_SCREEN, true, "Area 2");
        public static readonly GameScene LEVEL_3 = Create("ContraLv3", true, GameControlState.INFO_SCREEN, false, "Area 3");
        public static readonly GameScene LEVEL_4 = Create("ContraLv4", true, GameControlState.INFO_SCREEN, false, "Area 4");
        public static readonly GameScene LEVEL_5 = Create("ContraLv5", true, GameControlState.INFO_SCREEN, false, "Area 5");
        public static readonly GameScene LEVEL_6 = Create("ContraLv6", true, GameControlState.INFO_SCREEN, true, "Area 6");
        public static readonly GameScene LEVEL_7 = Create("ContraLv7", true, GameControlState.INFO_SCREEN, false, "Area 7");
        public static readonly GameScene LEVEL_8 = Create("ContraLv8", true, GameControlState.INFO_SCREEN, false, "Area 8");

        public string name;
        public int index;
        public bool hasInfoScreen;
        public GameControlState initialControlState;
        public bool moveXZ = false;
        public string introText;

        private static readonly Dictionary<string, GameScene> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameScene> All()
        {
            return new List<GameScene>()
            {
                TITLE_SCREEN,
                LEVEL_1,
                LEVEL_2,
                LEVEL_3,
                LEVEL_4,
                LEVEL_5,
                LEVEL_6,
                LEVEL_7,
                LEVEL_8,
            };
        }

        private static GameScene Create(string name, bool hasInfoScreen, GameControlState initialControlState, bool moveXZ, string introText)
        {
            GameScene layer = new GameScene
            {
                name = name,
                hasInfoScreen = hasInfoScreen,
                initialControlState = initialControlState,
                moveXZ = moveXZ,
                introText = introText,
            };
            return layer;
        }

        public static void AssignIndexesOnAwake()
        {
            Dictionary<string, int> sceneAndIndexInBuildSettings = UnityFn.GetSceneNameAndIndexDictInBuildSettings();
            SelectedIds scenesDefined = SelectedIds.Create().SelectAll(Fn.Map(s => s.name, All()));
            SelectedIds scenesMissing = scenesDefined.ImmutableRemoveAll(new List<string>(sceneAndIndexInBuildSettings.Keys));
            if (!scenesMissing.IsEmpty()) Debug.LogError(string.Join(", ", scenesMissing.GetIds()));
            foreach (GameScene scene in All()) scene.index = sceneAndIndexInBuildSettings[scene.name];
        }

        public static GameScene GetActiveScene()
        {
            return GetByName(SceneManager.GetActiveScene().name);
        }

        public static GameScene GetByName(string name)
        {
            return typeMap[(name)];
        }
    }
}