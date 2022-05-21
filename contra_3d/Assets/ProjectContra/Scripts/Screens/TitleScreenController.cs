using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ProjectContra.Scripts.Screens
{
    /// <summary>
    /// Used when there is no player in the game. When there are players in the game, this is no longer used.
    /// </summary>
    public class TitleScreenController : MonoBehaviour
    {
        private GameStoreData storeData;

        [SerializeField] public GameObject titleScreenDefaultButton;
        public GameObject menuCanvas;
        public GameObject lobbyCanvas;

        public bool canGoToTitleScreenFromLobby = true;

        private void Start()
        {
            storeData = AppResource.instance.storeData;
            menuCanvas.SetActive(true);
            lobbyCanvas.SetActive(false);
            storeData.controlState = GameControlState.TITLE_SCREEN_MENU;
        }

        private void Update()
        {
            if ((storeData.controlState == GameControlState.TITLE_SCREEN_LOBBY) && !storeData.inputManagerData.HasPlayers())
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame) OnSelectedBackFromLobby();
                if (Gamepad.current.buttonEast.wasPressedThisFrame) OnSelectedBackFromLobby();
            }
        }

        public void OnSelectedStartFromMenu()
        {
            menuCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
            UnityFn.SetTimeout(this, 0.1f, () => storeData.controlState = GameControlState.TITLE_SCREEN_LOBBY);
        }

        public void OnSelectedContinueFromMenu()
        {
            menuCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
            UnityFn.SetTimeout(this, 0.1f, () => storeData.controlState = GameControlState.TITLE_SCREEN_LOBBY);
        }

        public void OnSelectedBackFromLobby()
        {
            if (!canGoToTitleScreenFromLobby) return;
            menuCanvas.SetActive(true);
            lobbyCanvas.SetActive(false);
            storeData.controlState = GameControlState.TITLE_SCREEN_MENU;
            UnityFn.SetTimeout(this, 0.1f, () => EventSystem.current.SetSelectedGameObject(titleScreenDefaultButton));
        }

        public void OnSelectedQuit()
        {
            Application.Quit();
        }
    }
}