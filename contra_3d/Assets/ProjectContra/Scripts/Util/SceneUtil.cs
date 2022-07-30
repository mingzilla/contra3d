using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;

namespace ProjectContra.Scripts.Util
{
    public static class SceneUtil
    {
        public static void InitializeScene()
        {
            GameStoreData storeData = AppResource.instance.storeData.Init(GameScene.GetActiveScene());
            GameScene scene = storeData.currentScene;
            if (scene.hasInfoScreen) InfoScreenCanvasController.GetInstance().Init(scene.introText);
            if (scene.IsEnding()) EndingScreenController.GetInstance().Init();
        }
    }
}