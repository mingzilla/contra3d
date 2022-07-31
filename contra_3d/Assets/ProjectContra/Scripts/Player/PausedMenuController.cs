using System.Collections.Generic;
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

        public static PausedMenuController GetInstance()
        {
            if (instance != null) return instance;
            instance = Instantiate(AppResource.instance.pauseMenuPrefab, Vector3.zero, Quaternion.identity).GetComponent<PausedMenuController>().Init();
            return instance;
        }

        /// <summary>
        /// Not using Start() or Update() because at this point the system is paused, so time is stopped.
        /// </summary>
        private PausedMenuController Init()
        {
            storeData = AppResource.instance.storeData;
            storeData.controlState = GameControlState.IN_GAME_PAUSED;

            resumeButton.onClick.AddListener(HandleUnPause);
            restartLevelButton.onClick.AddListener(OnSelectedRestartLevel);
            quitButton.onClick.AddListener(OnSelectedQuit);
            buttons = new List<Button>() {resumeButton, restartLevelButton, quitButton};

            MoveToButton(currentButtonIndex);
            return this;
        }

        private void MoveToButton(int index)
        {
            currentButtonIndex = index;
            UnityFn.DeSelectButtons(buttons);
            UnityFn.MoveToButton(buttons[index]);
        }

        public void HandleUnPause()
        {
            storeData = GameFn.HandleUnPause(storeData);
        }

        public void OnSelectedRestartLevel()
        {
            UnityFn.RunWithInterval(AppResource.instance, buttonIntervalState, () =>
            {
                AppMusic.instance.Stop();
                storeData.ReloadScene();
                AppResource.instance.pauseMenuEventSystem.SetActive(false);
            });
        }

        public void OnSelectedQuit()
        {
            UnityFn.RunWithInterval(AppResource.instance, buttonIntervalState, () =>
            {
                UnityFn.QuitToMenu<PlayerController>();
                AppResource.instance.pauseMenuEventSystem.SetActive(false);
            });
        }
    }
}