using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Util
{
    public static class SceneUtil
    {
        private static bool canLoadScene = true;

        public static void InitializeScene()
        {
            GameStoreData storeData = AppResource.instance.storeData.Init(GameScene.GetActiveScene());
            GameScene scene = storeData.currentScene;
            if (scene.hasInfoScreen)
            {
                GameObject infoScreen = AppResource.instance.infoScreen;
                infoScreen.GetComponent<InfoScreenCanvasController>().Init(scene.introText);
            }
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
    }
}