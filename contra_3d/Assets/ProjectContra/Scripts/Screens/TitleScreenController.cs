using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using TMPro;
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
        [SerializeField] public GameObject titleScreenFullScreenButtonText;
        public GameObject menuCanvas;
        public GameObject lobbyCanvas;

        public bool canGoToTitleScreenFromLobby = true;
        private readonly IntervalState menuIntervalState = IntervalState.Create(0.2f);
        private TextMeshProUGUI fullScreenButtonText;

        private void Start()
        {
            storeData = AppResource.instance.storeData;
            menuCanvas.SetActive(true);
            lobbyCanvas.SetActive(false);
            storeData.controlState = GameControlState.TITLE_SCREEN_MENU;
            fullScreenButtonText = titleScreenFullScreenButtonText.GetComponent<TextMeshProUGUI>();
            SetFullScreenButtonText(Screen.fullScreen);
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

        public void OnSelectedToggleFullScreenFromMenu()
        {
            UnityFn.RunWithInterval(this, menuIntervalState, () =>
            {
                bool desireValue = !Screen.fullScreen;
                SetFullScreenButtonText(desireValue); // run this before setting Screen.fullScreen, because the result is reflected with some delay
                Screen.fullScreen = desireValue;
            });
        }

        private void SetFullScreenButtonText(bool currentlyIsFull)
        {
            fullScreenButtonText.text = currentlyIsFull ? "Window Mode" : "Full Screen";
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