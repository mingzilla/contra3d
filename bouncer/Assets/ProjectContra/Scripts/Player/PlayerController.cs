using System;
using System.Collections;
using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;


namespace ProjectContra.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public GameObject characterInGamePrefab;
        public GameObject characterInMenuPrefab;

        private CharacterInGameController inGameController;
        private CharacterInMenuController inMenuController;
        private UserInput userInput;

        private GameControlState currentControlState;

        private void Start()
        {
            const int playerId = 0; // todo - use correct userIndex

            userInput = UserInput.Create(playerId);
            currentControlState = GameControlState.IN_GAME; // todo - change this depending on situation

            inGameController = UnityFn.InstantiateDisabledCharacterObject<CharacterInGameController>(characterInGamePrefab);
            inMenuController = UnityFn.InstantiateDisabledCharacterObject<CharacterInMenuController>(characterInMenuPrefab);

            inGameController.Init(playerId);
            inMenuController.Init();

            AllocateControlObject(currentControlState);
        }

        void Update()
        {
            if (currentControlState == GameControlState.IN_GAME) inGameController.HandleUpdate(userInput);
            if (currentControlState == GameControlState.TITLE_SCREEN_MENU) inMenuController.HandleUpdate(userInput);
            UserInput.ResetTriggers(userInput);
        }

        void AllocateControlObject(GameControlState controlState)
        {
            if (controlState == GameControlState.IN_GAME) UnityFn.SetActiveAndDeActivateOthers(inGameController.gameObject, new List<GameObject>() {inMenuController.gameObject});
            if (controlState == GameControlState.TITLE_SCREEN_MENU) UnityFn.SetActiveAndDeActivateOthers(inMenuController.gameObject, new List<GameObject>() {inGameController.gameObject});
        }

        public void Move(InputAction.CallbackContext context)
        {
            userInput = UserInput.Move(userInput, context);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (context.started) userInput.jump = true;
            if (context.canceled) userInput.jumpCancelled = true;
        }
    }
}