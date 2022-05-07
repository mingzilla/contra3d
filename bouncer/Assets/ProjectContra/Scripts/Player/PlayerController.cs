using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
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
        public GameObject characterInLobbyPrefab;

        private GameStoreData storeData;
        private int playerId;

        private CharacterInLobbyController inLobbyController;
        private InfoScreenCanvasController infoScreenCanvasController;
        private CharacterInGameController inGameController;

        private UserInput userInput;

        private void Awake()
        {
            UnityFn.KeepAlive(gameObject);
        }

        private void Start()
        {
            storeData = AppResource.instance.storeData;
            playerId = gameObject.GetComponent<PlayerInput>().playerIndex;
            inLobbyController = UnityFn.InstantiateDisabledCharacterObject<CharacterInLobbyController>(characterInLobbyPrefab);
            infoScreenCanvasController = AppResource.instance.infoScreen.GetComponent<InfoScreenCanvasController>();
            inGameController = UnityFn.InstantiateDisabledCharacterObject<CharacterInGameController>(characterInGamePrefab);

            userInput = UserInput.Create(playerId);
            inGameController.Init(playerId);
        }

        void FixedUpdate()
        {
            GameControlState currentControlState = storeData.controlState;
            SetControlObjectActiveState();
            if (currentControlState == GameControlState.TITLE_SCREEN_LOBBY) inLobbyController.HandleUpdate(playerId, userInput, new List<GameObject>() {inLobbyController.gameObject, inGameController.gameObject, gameObject});
            if (currentControlState == GameControlState.INFO_SCREEN) infoScreenCanvasController.HandleUpdate(userInput);
            if (currentControlState == GameControlState.IN_GAME) inGameController.HandleUpdate(userInput);
            UserInput.ResetTriggers(userInput);
        }

        void SetControlObjectActiveState()
        {
            GameControlState controlState = storeData.controlState;
            if (controlState == GameControlState.TITLE_SCREEN_LOBBY) UnityFn.SetActiveAndDeActivateOthers(inLobbyController.gameObject, GetControlObjectsExcept(inLobbyController));
            if (controlState == GameControlState.INFO_SCREEN) UnityFn.SetActiveAndDeActivateOthers(AppResource.instance.infoScreen, GetControlObjectsExcept(infoScreenCanvasController));
            if (controlState == GameControlState.IN_GAME) UnityFn.SetActiveAndDeActivateOthers(inGameController.gameObject, GetControlObjectsExcept(inGameController));
        }

        private List<GameObject> GetControlObjectsExcept(MonoBehaviour exclude)
        {
            List<GameObject> list = new List<GameObject>() {inLobbyController.gameObject, inGameController.gameObject, AppResource.instance.infoScreen};
            list.Remove(exclude.gameObject);
            return list;
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

        public void Space(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            if (context.started) userInput.space = true;
        }

        public void Escape(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            if (context.started) userInput.escape = true;
        }

        public void Pause(InputAction.CallbackContext context)
        {
            if (userInput == null) return;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return;
            if (context.started) userInput.pause = true;
        }

        public void NextLevel(InputAction.CallbackContext context)
        {
            SceneUtil.LoadNextScene(AppResource.instance);
        }
    }
}