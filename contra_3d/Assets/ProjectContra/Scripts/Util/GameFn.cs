using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.PlayerManagement;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
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
            return storeData;
        }

        public static GameStoreData HandleUnPause(GameStoreData storeData)
        {
            AppSfx.Play(AppSfx.instance.pause);
            AppMusic.instance.UnPause();
            storeData.controlState = GameControlState.IN_GAME;
            UnityFn.UnPause();
            AppResource.instance.pauseMenuEventSystem.SetActive(false);
            return storeData;
        }

        /// <summary>
        /// Handles player control, and prevent menu control because menu control should be handled by EventSystem
        /// </summary>
        public static bool CanControlPlayer(GameStoreData storeData, UserInput userInput)
        {
            if (storeData == null) return false;
            if (storeData.IsPaused()) return false;
            if (!UserInput.CanControl(userInput)) return false;
            return true;
        }
        public static bool CanControlPlayerOnContextStarted(GameStoreData storeData, UserInput userInput, InputAction.CallbackContext context)
        {
            if (storeData == null) return false;
            if (storeData.IsPaused()) return false;
            if (!UserInput.CanControlOnContextStarted(userInput, context)) return false;
            return true;
        }
    }
}