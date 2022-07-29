using System;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using BaseUtil.GameUtil.PlayerManagement;
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
        private readonly IntervalState pauseInterval = IntervalState.Create(0.1f);
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
            if (userInput == null) return;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            userInput = UserInput.Move(userInput, context);
            if (context.started) HandleInputWhenPaused(); // if not in context.started, it runs 3 times
        }

        public void Jump(InputAction.CallbackContext context)
        {
            RunWhenContextStarted(context, () => userInput.jump = true);
            if (context.canceled) userInput.jumpCancelled = true;
        }

        public void Fire1(InputAction.CallbackContext context)
        {
            RunWhenContextStarted(context, () => userInput.fire1 = true);
        }

        public void Space(InputAction.CallbackContext context)
        {
            RunWhenContextStarted(context, () => userInput.space = true);
        }

        public void Escape(InputAction.CallbackContext context)
        {
            if (storeData.controlState == GameControlState.IN_GAME) return; // if it's in game, this is the same as Pause(), so don't double run
            RunWhenContextStarted(context, () => userInput.escape = true);
        }

        public void Pause(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            if (context.started && storeData.controlState == GameControlState.IN_GAME)
            {
                UnityFn.RunWithInterval(AppResource.instance, pauseInterval, () =>
                {
                    AppSfx.Play(AppSfx.instance.pause);
                    AppMusic.instance.Pause();
                    storeData.controlState = GameControlState.IN_GAME_PAUSED;
                    SceneInitData sceneInitData = AppResource.instance.GetCurrentSceneInitData();
                    GameScene gameScene = AppResource.instance.GetCurrentScene();
                    controlObjectData.SetControlObjectActiveState(playerId, storeData.controlState, characterInGamePrefab, characterInXzGamePrefab, characterInLobbyPrefab, gameScene, sceneInitData);
                    UnityFn.Pause();
                });
            }
        }

        private void RunWhenContextStarted(InputAction.CallbackContext context, Action fn)
        {
            if (userInput == null) return;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            if (context.started)
            {
                fn();
                HandleInputWhenPaused();
            }
        }

        /// <summary>
        /// When paused, the update loop is stopped, so nothing runs inside update loop.
        /// </summary>
        private void HandleInputWhenPaused()
        {
            if (!storeData.IsPaused()) return;
            controlObjectData.pausedMenuController.HandleUpdate(userInput);
            UserInput.ResetTriggers(userInput); // same as update loop, need to reset userInput after execution
        }

        public void NextLevel(InputAction.CallbackContext context)
        {
            SceneUtil.ReloadScene(AppResource.instance);
        }

        private void OnDestroy()
        {
            isBroken = true;
        }
    }
}