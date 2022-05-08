using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectContra.Scripts.Player
{
    public class PausedMenuController : MonoBehaviour
    {
        private static PausedMenuController instance;
        private GameStoreData storeData;

        [SerializeField] private Button lobbyButton;
        [SerializeField] private Button quitButton;
        public List<Button> buttons;
        private int currentButtonIndex = 0;

        public static PausedMenuController GetInstance()
        {
            if (instance != null) return instance;
            instance = Instantiate(AppResource.instance.pauseMenuPrefab, Vector3.zero, Quaternion.identity).GetComponent<PausedMenuController>().Init();
            return instance;
        }

        private PausedMenuController Init()
        {
            storeData = AppResource.instance.storeData;
            storeData.controlState = GameControlState.IN_GAME_PAUSED;
            lobbyButton.onClick.AddListener(OnSelectedLobby);
            quitButton.onClick.AddListener(OnSelectedQuit);
            buttons = new List<Button>() {lobbyButton, quitButton};
            SelectButton(currentButtonIndex);
            return this;
        }

        public void HandleUpdate(UserInput userInput)
        {
            if (userInput.up || userInput.down) MoveSelection(userInput);
            if (userInput.fire1 || userInput.space) Ok();
            if (userInput.jump || userInput.escape) HandleUnPause();
        }

        void MoveSelection(UserInput userInput)
        {
            if (userInput.down)
            {
                int nextIndex = FnVal.GetNextIndex(currentButtonIndex, buttons.Count);
                SelectButton(nextIndex);
            }
            if (userInput.up)
            {
                int nextIndex = FnVal.GetPreviousIndex(currentButtonIndex, buttons.Count);
                SelectButton(nextIndex);
            }
        }

        private void SelectButton(int index)
        {
            SetIndex(index);
            Button button = buttons[index];
            button.Select(); // Or EventSystem.current.SetSelectedGameObject(myButton.gameObject) - Select Button
            button.OnSelect(null); // Or myButton.OnSelect(new BaseEventData(EventSystem.current)) - Highlight Button
        }

        private void SetIndex(int index)
        {
            currentButtonIndex = index;
        }

        public void Ok()
        {
            buttons[currentButtonIndex].onClick.Invoke();
        }

        private void HandleUnPause()
        {
            storeData.controlState = GameControlState.IN_GAME;
            Time.timeScale = 1f;
        }

        public void OnSelectedLobby()
        {
            Debug.Log("Lobby");
        }

        public void OnSelectedQuit()
        {
            SceneManager.LoadScene(0);
        }
    }
}