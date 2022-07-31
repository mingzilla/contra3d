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
            controlObjectData.SetControlObjectActiveState(playerId, currentControlState, characterInGamePrefab, characterInXzGamePrefab, characterInLobbyPrefab, gameScene, sceneInitData);
            if (currentControlState == GameControlState.TITLE_SCREEN_LOBBY) controlObjectData.inLobbyController.HandleUpdate(playerId, userInput, gameObject);
            if (currentControlState == GameControlState.INFO_SCREEN) controlObjectData.infoScreenCanvasController.HandleUpdate(userInput);
            if (currentControlState == GameControlState.IN_GAME && !gameScene.moveXZ) controlObjectData.inGameController.HandleUpdate(userInput);
            if (currentControlState == GameControlState.IN_GAME && gameScene.moveXZ) controlObjectData.inXzGameController.HandleUpdate(userInput);
            if (currentControlState == GameControlState.ENDING_SCREEN) controlObjectData.endingScreenController.HandleUpdate(userInput);
            UserInput.ResetTriggers(userInput);
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (!UserInput.CanControl(userInput)) return;
            userInput = UserInput.Move(userInput, context);
            if (context.started) HandleInputWhenPaused(); // if not in context.started, it runs 3 times
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (!UserInput.CanControl(userInput)) return;
            if (context.started) userInput.jump = true;
            if (context.canceled) userInput.jumpCancelled = true;
        }

        public void Fire1(InputAction.CallbackContext context)
        {
            if (!UserInput.CanControlContextStarted(userInput, context)) return;
            userInput.fire1 = true;
            HandleInputWhenPaused();
        }

        public void Space(InputAction.CallbackContext context)
        {
            if (!UserInput.CanControlContextStarted(userInput, context)) return;
            userInput.space = true;
            HandleInputWhenPaused();
        }

        public void Escape(InputAction.CallbackContext context)
        {
            if (!storeData.IsPaused()) return;
            if (!UserInput.CanControlContextStarted(userInput, context)) return;
            userInput.escape = true;
            HandleInputWhenPaused();
        }

        public void Pause(InputAction.CallbackContext context)
        {
            if (!UserInput.CanControlContextStarted(userInput, context)) return;

            UnityFn.RunWithInterval(AppResource.instance, buttonIntervalState, () =>
            {
                if (storeData.IsPaused())
                {
                    storeData = GameFn.HandleUnPause(storeData);
                }
                else
                {
                    storeData = GameFn.HandlePause(storeData);
                    controlObjectData.SetControlObjectActiveState(playerId, GameControlState.IN_GAME_PAUSED, characterInGamePrefab, characterInXzGamePrefab, characterInLobbyPrefab,
                        AppResource.instance.GetCurrentScene(),
                        AppResource.instance.GetCurrentSceneInitData());
                }
            });
        }

        /// <summary>
        /// When paused, the update loop is stopped, so nothing runs inside update loop.
        /// </summary>
        private void HandleInputWhenPaused()
        {
            if (!storeData.IsPaused()) return;
            controlObjectData.pausedMenuController.HandleInput(userInput);
            UserInput.ResetTriggers(userInput); // same as update loop, need to reset userInput after execution
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