using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player.Domain;
using ProjectContra.Scripts.Screens;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectContra.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject characterInGamePrefab;
        public GameObject characterInLobbyPrefab;

        private InfoScreenCanvasController infoScreenCanvasController;
        private CharacterInLobbyController inLobbyController;
        private CharacterInGameController inGameController;
        private GameStoreData storeData;
        private int playerId;

        private UserInput userInput;

        private void Start()
        {
            infoScreenCanvasController = AppResource.instance.infoScreen.GetComponent<InfoScreenCanvasController>();
            inGameController = UnityFn.InstantiateDisabledCharacterObject<CharacterInGameController>(characterInGamePrefab);
            inLobbyController = UnityFn.InstantiateDisabledCharacterObject<CharacterInLobbyController>(characterInLobbyPrefab);
            storeData = AppResource.instance.storeData;
            playerId = gameObject.GetComponent<PlayerInput>().playerIndex;

            userInput = UserInput.Create(playerId);
            inGameController.Init(playerId);
        }

        void FixedUpdate()
        {
            GameControlState currentControlState = storeData.controlState;
            if (currentControlState == GameControlState.INFO_SCREEN) infoScreenCanvasController.HandleUpdate(userInput, () => AllocateControlObject(GameControlState.IN_GAME));
            if (currentControlState == GameControlState.TITLE_SCREEN_LOBBY) inLobbyController.HandleUpdate(playerId, userInput);
            if (currentControlState == GameControlState.IN_GAME) inGameController.HandleUpdate(userInput);
            UserInput.ResetTriggers(userInput);
        }

        void AllocateControlObject(GameControlState controlState)
        {
            if (controlState == GameControlState.TITLE_SCREEN_LOBBY) UnityFn.SetActiveAndDeActivateOthers(inLobbyController.gameObject, new List<GameObject>() {inGameController.gameObject});
            if (controlState == GameControlState.IN_GAME) UnityFn.SetActiveAndDeActivateOthers(inGameController.gameObject, new List<GameObject>() {inLobbyController.gameObject});
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            userInput = UserInput.Move(userInput, context);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            if (context.started) userInput.jump = true;
            if (context.canceled) userInput.jumpCancelled = true;
        }

        public void Fire1(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            if (context.started) userInput.fire1 = true;
        }

        public void Pause(InputAction.CallbackContext context)
        {
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            // userInput.pause = true;
        }

        public void NextLevel(InputAction.CallbackContext context)
        {
            GameScene.TransitionToNextLevel(this, state => storeData.controlState = state);
        }
    }
}