using System.IO;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectContra.Scripts.Util
{
    public static class SceneUtil
    {
        private static bool canLoadScene = true;

        public static void InitializeScene()
        {
            GameStoreData storeData = AppResource.instance.storeData.Init(GameScene.GetActiveScene());
            GameScene scene = storeData.currentScene;
            if (scene.hasInfoScreen) InfoScreenCanvasController.GetInstance().Init(scene.introText);
            if (scene.IsEnding()) EndingScreenController.GetInstance().Init();
        }

        public static void LoadNextScene(MonoBehaviour controller)
        {
            if (canLoadScene)
            {
                canLoadScene = false;
                UnityFn.SetTimeout(controller, 2, () => canLoadScene = true);
                UnityFn.LoadNextScene();
            }
        }

        public static void ReloadScene(MonoBehaviour controller)
        {
            if (canLoadScene)
            {
                canLoadScene = false;
                UnityFn.SetTimeout(controller, 2, () => canLoadScene = true);
                UnityFn.ReloadCurrentScene();
            }
        }

        /// <summary>
        /// Destroy all players and quit to menu
        /// </summary>
        /// <typeparam name="T">Player Controller, which represents user input control</typeparam>
        public static void QuitToMenu<T>(int menuSceneIndex = 0) where T : MonoBehaviour
        {
            T[] objects = Object.FindObjectsOfType<T>();
            Fn.EachInArray(x => Object.Destroy(x.gameObject), objects);
            SceneManager.LoadScene(menuSceneIndex);
        }
    }
}