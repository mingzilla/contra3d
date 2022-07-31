using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using ProjectContra.Scripts.Util;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectContra.Scripts.Player
{
    public class PausedMenuController : MonoBehaviour
    {
        private static PausedMenuController instance;
        private GameStoreData storeData;

        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartLevelButton;
        [SerializeField] private Button quitButton;
        private List<Button> buttons;
        private int currentButtonIndex = 0;
        private readonly IntervalState buttonIntervalState = IntervalState.Create(0.2f);

        private UserInput userInput = UserInput.Create(0); // all players share the same userInput object

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

            resumeButton.onClick.AddListener(OnSelectedResume);
            restartLevelButton.onClick.AddListener(OnSelectedRestartLevel);
            quitButton.onClick.AddListener(OnSelectedQuit);
            buttons = new List<Button>() {resumeButton, restartLevelButton, quitButton};

            MoveToButton(currentButtonIndex);
            return this;
        }

        public void HandleInput(UserInput userInput)
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
                MoveToButton(nextIndex);
            }
            if (userInput.up)
            {
                int nextIndex = FnVal.GetPreviousIndex(currentButtonIndex, buttons.Count);
                MoveToButton(nextIndex);
            }
        }

        private void MoveToButton(int index)
        {
            currentButtonIndex = index;
            UnityFn.DeSelectButtons(buttons);
            UnityFn.MoveToButton(buttons[index]);
        }

        public void Ok()
        {
            UnityFn.TriggerButton(buttons[currentButtonIndex]);
        }

        private void HandleUnPause()
        {
            storeData = GameFn.HandleUnPause(storeData);
        }

        public void OnSelectedResume()
        {
            HandleUnPause();
        }

        public void OnSelectedRestartLevel()
        {
            UnityFn.UnPause(); // when paused, time is stopped so 
            UnityFn.RunWithInterval(AppResource.instance, buttonIntervalState, () => storeData.ReloadScene());
        }

        public void OnSelectedQuit()
        {
            UnityFn.UnPause();
            UnityFn.RunWithInterval(AppResource.instance, buttonIntervalState, () => UnityFn.QuitToMenu<PlayerController>());
        }
    }
}