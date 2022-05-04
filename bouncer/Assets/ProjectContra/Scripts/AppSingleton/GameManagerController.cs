using System;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Screens;
using UnityEngine;
using ProjectContra.Scripts.Types;
using UnityEngine.SceneManagement;

namespace ProjectContra.Scripts.AppSingleton
{
    /// <summary>
    /// Manages players and loading levels
    /// </summary>
    public class GameManagerController : MonoBehaviour
    {
        public static GameManagerController instance;

        private GameStoreData storeData;

        /// <summary>
        /// Called when scene is loaded, so when loading another scene, it's called again
        /// </summary>
        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
            GameTag.InitOnAwake();
            GameLayer.InitOnAwake();
            // GameScene.AssignIndexesOnAwake();
            
            Debug.Log("GameManagerController.Awake");
        }

        private void OnEnable()
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