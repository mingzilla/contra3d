using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;
using ProjectContra.Scripts.Types;

namespace ProjectContra.Scripts.AppSingleton
{
    /// <summary>
    /// Manages players and loading levels
    /// </summary>
    public class GameManagerController : MonoBehaviour
    {
        public static GameManagerController instance;

        private GameStoreData storeData;

        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
            GameTag.InitOnAwake();
            GameLayer.InitOnAwake();
        }

        public void HandleUpdate(UserInput userInput)
        {
            if (userInput.fire1) HandleTransitionToGame(userInput);
        }

        public void HandleTransitionToGame(UserInput userInput)
        {
            AppResource.instance.SetControlState(GameControlState.IN_GAME);
            AppResource.instance.infoScreen.SetActive(false);
        }
    }
}