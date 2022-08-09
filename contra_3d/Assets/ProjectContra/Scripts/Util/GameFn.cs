using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.PlayerManagement;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectContra.Scripts.Util
{
    /// <summary>
    /// Similar to UnityFn, but this one has specific logic to this game
    /// </summary>
    public static class GameFn
    {
        public static GameStoreData HandlePause(GameStoreData storeData)
        {
            AppSfx.Play(AppSfx.instance.pause);
            AppMusic.instance.Pause();
            storeData.controlState = GameControlState.IN_GAME_PAUSED;
            UnityFn.Pause();
            AppResource.instance.pauseMenuEventSystem.SetActive(true);
            PausedMenuController.GetInstance(); // creates the menu object, this has to be done after activating event system, if activating later, it won't know presence of buttons
            return storeData;
        }

        public static GameStoreData HandleUnPause(GameStoreData storeData)
        {
            AppSfx.Play(AppSfx.instance.pause);
            AppMusic.instance.UnPause();
            storeData.controlState = GameControlState.IN_GAME;
            UnityFn.UnPause();
            AppResource.instance.pauseMenuEventSystem.SetActive(false);
            UnityFn.SafeDestroy(Object.FindObjectOfType<PausedMenuController>()?.gameObject); // destroy menu
            return storeData;
        }

        /// <summary>
        /// Handles player control, and prevent menu control because menu control should be handled by EventSystem
        /// </summary>
        public static bool CanControlPlayer(GameStoreData storeData, UserInput userInput)
        {
            if (storeData == null) return false;
            if (storeData.IsPaused()) return false;
            if (userInput == null) return false;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return false;
            return true;
        }

        /// <summary>
        /// For buttons that are not continuous. e.g. non movement buttons
        /// </summary>
        public static bool CanControlPlayerOnContextStarted(GameStoreData storeData, UserInput userInput, InputAction.CallbackContext context)
        {
            if (storeData == null) return false;
            if (storeData.IsPaused()) return false;
            if (userInput == null) return false;
            if (!PlayerInputManagerData.CurrentDeviceIsPaired()) return false;
            if (!context.started) return false; // only handle context.started, otherwise it runs 3 times
            return true;
        }

        public static void ReloadScene(GameStoreData storeData)
        {
            AppMusic.instance.Stop();
            storeData.ReloadScene();
        }

        public static void LoadNextScene()
        {
            AppMusic.instance.Stop();
            UnityFn.LoadNextScene();
        }
    }
}