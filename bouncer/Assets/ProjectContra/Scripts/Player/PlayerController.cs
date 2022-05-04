using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Screens;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectContra.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject characterInGamePrefab;
        public GameObject characterInMenuPrefab;

        private InfoScreenCanvasController infoScreenCanvasController;
        private CharacterInMenuController inMenuController;
        private CharacterInGameController inGameController;
        private GameStoreData storeData;

        private UserInput userInput;

        private void Start()
        {
            infoScreenCanvasController = AppResource.instance.infoScreen.GetComponent<InfoScreenCanvasController>();
            inGameController = UnityFn.InstantiateDisabledCharacterObject<CharacterInGameController>(characterInGamePrefab);
            inMenuController = UnityFn.InstantiateDisabledCharacterObject<CharacterInMenuController>(characterInMenuPrefab);
            storeData = AppResource.instance.storeData;

            int playerId = storeData.GetNextPlayerId();
            userInput = UserInput.Create(playerId);
            inGameController.Init(playerId);
            inMenuController.Init();
        }

        void FixedUpdate()
        {
            GameControlState currentControlState = storeData.controlState;
            if (currentControlState == GameControlState.INFO_SCREEN) infoScreenCanvasController.HandleUpdate(userInput, () => AllocateControlObject(GameControlState.IN_GAME));
            if (currentControlState == GameControlState.LOBBY_SCREEN) inMenuController.HandleUpdate(userInput);
            if (currentControlState == GameControlState.IN_GAME) inGameController.HandleUpdate(userInput);
            UserInput.ResetTriggers(userInput);
        }

        void AllocateControlObject(GameControlState controlState)
        {
            if (controlState == GameControlState.LOBBY_SCREEN) UnityFn.SetActiveAndDeActivateOthers(inMenuController.gameObject, new List<GameObject>() {inGameController.gameObject});
            if (controlState == GameControlState.IN_GAME) UnityFn.SetActiveAndDeActivateOthers(inGameController.gameObject, new List<GameObject>() {inMenuController.gameObject});
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            userInput = UserInput.Move(userInput, context);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            if (context.started) userInput.jump = true;
            if (context.canceled) userInput.jumpCancelled = true;
        }

        public void Fire1(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            if (context.started) userInput.fire1 = true;
        }

        public void Pause(InputAction.CallbackContext context)
        {
            // userInput.pause = true;
        }

        public void NextLevel(InputAction.CallbackContext context)
        {
            GameScene.TransitionToNextLevel(this, state => storeData.controlState = state);
        }
    }
}