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
        private static bool canLoadNextScene = true;

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
            if (canLoadNextScene)
            {
                canLoadNextScene = false;
                UnityFn.SetTimeout(controller, 2, () => canLoadNextScene = true);
                UnityFn.LoadNextScene();
            }
        }
    }
}