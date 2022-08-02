using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.EventSystems;

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
        private readonly IntervalState menuIntervalState = IntervalState.Create(0.2f);

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
                if (UserInput.IsMenuKeyboardCancel()) OnSelectedBackFromLobby();
                if (UserInput.IsMenuGamepadCancel()) OnSelectedBackFromLobby();
            }
        }

        public void OnSelectedStartFromMenu()
        {
            menuCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
            UnityFn.RunWithInterval(this, menuIntervalState, () => storeData.controlState = GameControlState.TITLE_SCREEN_LOBBY);
        }

        public void OnSelectedContinueFromMenu()
        {
            menuCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
            UnityFn.RunWithInterval(this, menuIntervalState, () => storeData.controlState = GameControlState.TITLE_SCREEN_LOBBY);
        }

        public void OnSelectedBackFromLobby()
        {
            if (!canGoToTitleScreenFromLobby) return;
            menuCanvas.SetActive(true);
            lobbyCanvas.SetActive(false);
            storeData.controlState = GameControlState.TITLE_SCREEN_MENU;
            UnityFn.RunWithInterval(this, menuIntervalState, () => EventSystem.current.SetSelectedGameObject(titleScreenDefaultButton));
        }

        public void OnSelectedQuit()
        {
            Application.Quit();
        }
    }
}