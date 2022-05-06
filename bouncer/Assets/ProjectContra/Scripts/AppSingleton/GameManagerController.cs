using System;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player.Domain;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectContra.Scripts.AppSingleton
{
    /// <summary>
    /// Alive through out the whole game. GameManagerController is PlayerInputManagerController.
    /// It manages the game, which has to have human players, a human player == PlayerInput game objects == PlayerController.
    /// This uses other things, while other things use AppResource.
    /// </summary>
    public class GameManagerController : MonoBehaviour
    {
        public static GameManagerController instance;
        private GameStoreData storeData;
        private PlayerInputManagerData inputManagerData;

        /// <summary>
        /// Called when scene is loaded, so when loading another scene, it's called again.
        /// Note:
        /// - Awake - used to set up itself without using other controllers
        /// - OnEnable - runs before OtherController.Awake(), so can't use other singleton 
        /// - Start - can use other singleton
        /// </summary>
        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
            GameTag.InitOnAwake();
            GameLayer.InitOnAwake();
        }

        private void Start()
        {
            storeData = AppResource.instance.storeData;
            inputManagerData = storeData.inputManagerData;
            PlayerInputManager.instance.playerPrefab = AppResource.instance.playerPrefab;
        }

        /// <summary>
        /// Auto executed when PlayerInputManager.instance.JoinPlayer() runs without problems
        /// </summary>
        /// <param name="playerInput"></param>
        public void OnPlayerJoined(PlayerInput playerInput)
        {
            inputManagerData.AddPlayer(playerInput);
            Debug.Log("OnPlayerJoined " + playerInput.playerIndex);
        }

        /// <summary>
        /// Auto executed when Player.gameObject is Destroy()ed
        /// </summary>
        /// <param name="playerInput"></param>
        public void OnPlayerLeft(PlayerInput playerInput)
        {
            inputManagerData.RemovePlayer(playerInput);
            storeData.RemovePlayer(playerInput.playerIndex);
            storeData.canGoToTitleScreenFromLobby = false;
            UnityFn.SetTimeout(this, 0.1f, () =>
            {
                storeData.canGoToTitleScreenFromLobby = true;
            });
            Debug.Log("OnPlayerLeft " + playerInput.playerIndex);
        }

        public void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                JoinPlayer(Keyboard.current);
            }

            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                JoinPlayer(Gamepad.current);
            }
        }

        private void JoinPlayer(InputDevice device)
        {
            if (inputManagerData.CanJoinPlayer(storeData.controlState, device))
            {
                int playerId = storeData.GetNextPlayerId();
                PlayerInputManager.instance.JoinPlayer(playerId, -1, null, device);
            }
        }
    }
}