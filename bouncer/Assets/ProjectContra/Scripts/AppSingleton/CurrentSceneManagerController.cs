using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton
{
    /// <summary>
    /// Manages the current scene. Need instantiation per scene so that the relevant content is loaded.
    /// Don't put this as part of GameManager, this object needs to be destroyable
    /// </summary>
    public class CurrentSceneManagerController : MonoBehaviour
    {
        private GameStoreData storeData;

        private void Start()
        {
            storeData = AppResource.instance.storeData.Init(GameScene.GetActiveScene());
            InitializeScene();
        }

        private void InitializeScene()
        {
            GameScene scene = storeData.currentScene;
            if (scene.hasInfoScreen)
            {
                GameObject infoScreen = AppResource.instance.infoScreen;
                infoScreen.GetComponent<InfoScreenCanvasController>().Init(scene.introText);
                infoScreen.SetActive(true);
            }
        }
    }
}