﻿using BaseUtil.GameUtil;
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

        /// <summary>
        /// Consistently using AppResource to set up coroutine, so that it can be stopped if needed. e.g. player killed after boss killed 
        /// </summary>
        public static void LoadNextSceneAfterBossKilled(GameObject bossGameObject, bool isLast)
        {
            UnityFn.SetTimeout(AppResource.instance, 5, () =>
            {
                if (!isLast) AppSfx.instance.levelClear.Play();
                if (isLast) AppSfx.instance.allLevelsClear.Play();
                UnityFn.SetTimeout(AppResource.instance, 5, () =>
                {
                    bool allPlayersDead = AppResource.instance.storeData.AllPlayersDead();
                    if (allPlayersDead) ReloadScene();
                    if (!allPlayersDead) LoadNextScene();
                });
                Object.Destroy(bossGameObject);
            });
        }

        public static void ReloadScene()
        {
            GameStoreData storeData = AppResource.instance.storeData;
            AppResource.instance.StopCoroutines(); // e.g. players killed after boss killed, this stops moving to the next level
            AppMusic.instance.Stop();
            storeData.ReloadScene();
        }

        public static void LoadNextScene()
        {
            AppResource.instance.StopCoroutines(); // so that e.g. reload scene won't get executed by any chance
            AppMusic.instance.Stop();
            UnityFn.LoadNextScene();
        }

        public static void QuitToMenu()
        {
            AppResource.instance.StopCoroutines();
            AppMusic.instance.Stop();
            UnityFn.QuitToMenu<PlayerController>();
        }
    }
}