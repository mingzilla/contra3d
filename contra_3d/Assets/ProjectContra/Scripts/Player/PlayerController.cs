using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
using ProjectContra.Scripts.Player.Domain;
using ProjectContra.Scripts.Types;
using ProjectContra.Scripts.Util;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectContra.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject characterInGamePrefab;
        public GameObject characterInXzGamePrefab;
        public GameObject characterInLobbyPrefab;

        private GameStoreData storeData;
        private readonly PlayerControlObjectData controlObjectData = PlayerControlObjectData.Create();
        private int playerId;

        private UserInput userInput;
        private readonly IntervalState buttonIntervalState = IntervalState.Create(0.2f);
        public bool isBroken = false;

        private void Awake()
        {
            UnityFn.KeepAlive(gameObject);
        }

        private void Start()
        {
            storeData = AppResource.instance.storeData;
            playerId = gameObject.GetComponent<PlayerInput>().playerIndex;
            userInput = UserInput.Create(playerId);
        }

        void FixedUpdate()
        {
            SceneInitData sceneInitData = AppResource.instance.GetCurrentSceneInitData();
            GameScene gameScene = AppResource.instance.GetCurrentScene();
            GameControlState currentControlState = storeData.controlState;
            bool isAlive = storeData.GetPlayer(playerId).isAlive;
            controlObjectData.SetControlObjectActiveState(playerId, isAlive, currentControlState, characterInGamePrefab, characterInXzGamePrefab, characterInLobbyPrefab, gameScene, sceneInitData);
            if (currentControlState == GameControlState.TITLE_SCREEN_LOBBY) controlObjectData.inLobbyController.HandleUpdate(playerId, userInput, gameObject);
            if (currentControlState == GameControlState.INFO_SCREEN) controlObjectData.infoScreenCanvasController.HandleUpdate(userInput);
            if (currentControlState == GameControlState.IN_GAME && isAlive && !gameScene.moveXZ) controlObjectData.inGameController.HandleUpdate(userInput);
            if (currentControlState == GameControlState.IN_GAME && isAlive && gameScene.moveXZ) controlObjectData.inXzGameController.HandleUpdate(userInput);
            if (currentControlState == GameControlState.ENDING_SCREEN) controlObjectData.endingScreenController.HandleUpdate(userInput);
            UserInput.ResetTriggers(userInput);
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (!GameFn.CanControlPlayer(storeData, userInput)) return;
            userInput = UserInput.Move(userInput, context);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (!GameFn.CanControlPlayer(storeData, userInput)) return;
            if (context.started) userInput.jump = true;
            if (context.canceled) userInput.jumpCancelled = true;
        }

        public void Fire1(InputAction.CallbackContext context)
        {
            if (!GameFn.CanControlPlayerOnContextStarted(storeData, userInput, context)) return;
            userInput.fire1 = true;
        }

        public void Pause(InputAction.CallbackContext context)
        {
            if (!GameFn.CanControlPlayerOnContextStarted(storeData, userInput, context)) return;
            if (!storeData.IsInGame()) return; // prevent pause menu when it's not in game 
            if (!storeData.IsFirstPlayer(playerId)) return; // seems like only first player can use the event system 
            UnityFn.RunWithInterval(AppResource.instance, buttonIntervalState, () =>
            {
                storeData = GameFn.HandlePause(storeData);
            });
        }

        public void NextLevel(InputAction.CallbackContext context)
        {
            UnityFn.RunWithInterval(AppResource.instance, buttonIntervalState, UnityFn.LoadNextScene);
        }

        private void OnDestroy()
        {
            isBroken = true;
        }
    }
}