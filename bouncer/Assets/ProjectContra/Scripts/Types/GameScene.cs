using System.Collections.Generic;
using UnityEngine.SceneManagement;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace ProjectContra.Scripts.Types
{
    public class GameScene
    {
        public static readonly GameScene LEVEL_1 = Create("ContraLv1", "Area 1");
        public static readonly GameScene LEVEL_2 = Create("ContraLv2", "Area 2");

        public string name;
        public string introText;

        private static readonly Dictionary<string, GameScene> typeMap = Fn.ListToDictionaryWithKeyFn((x) => x.name, All());

        static List<GameScene> All()
        {
            return new List<GameScene>()
            {
                LEVEL_1,
                LEVEL_2,
            };
        }

        private static GameScene Create(string name, string introText)
        {
            GameScene layer = new GameScene
            {
                name = name,
                introText = introText,
            };
            return layer;
        }

        public static GameScene GetByName(string name)
        {
            return typeMap[(name)];
        }

        public static void InitializeLevel(MonoBehaviour controller, GameScene scene)
        {
            // disable everything apart from control -> hit A or Space to skip intro screen
            // disable scene content -> hit any button to enable and start, hide intro screen
            
            // intro screen is a canvas in the scene
        }

        public static void TransitionToNextLevel(MonoBehaviour controller, GameScene scene)
        {
            UnityFn.SetTimeout(controller, 5, () =>
            {
               // stop inputs
               // darken screen
               LoadLevel(scene);
            });
        }

        public static void LoadLevel(GameScene scene)
        {
            SceneManager.LoadScene(scene.name);
        }
    }
}