using System;
using System.Collections.Generic;
using System.Linq;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectContra.Scripts.Player.Domain
{
    public class PlayerInputManagerData
    {
        private Dictionary<int, PlayerInputAndStatus> indexAndPlayer = new Dictionary<int, PlayerInputAndStatus>();
        private static int maxPlayerCount = 4;

        public static PlayerInputManagerData Create()
        {
            return new PlayerInputManagerData();
        }

        public void AddPlayer(PlayerInput playerInput)
        {
            indexAndPlayer[playerInput.playerIndex] = PlayerInputAndStatus.Create(playerInput);
        }

        public void RemovePlayer(PlayerInput playerInput)
        {
            indexAndPlayer.Remove(playerInput.playerIndex);
        }

        public int GetPlayerIndex(GameObject playerGameObject)
        {
            PlayerInput playerInput = playerGameObject.GetComponent<PlayerInput>();
            return playerInput.playerIndex;
        }

        public bool HasPlayers()
        {
            return indexAndPlayer.Count > 0;
        }

        public bool CanJoinPlayer(GameControlState gameControlState, InputDevice device)
        {
            return (gameControlState == GameControlState.TITLE_SCREEN_LOBBY) && HasVacancy() && !DeviceIsPaired(device);
        }

        public int GetNextIndexToJoin()
        {
            for (int i = 0; i < maxPlayerCount; i++)
            {
                if (!indexAndPlayer.ContainsKey(i)) return i;
            }
            return 3;
        }

        public void SetPlayerReady(int playerIndex, bool isReady)
        {
            try
            {
                indexAndPlayer[(playerIndex)].isReady = isReady;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public bool AllPlayersAreReady()
        {
            return indexAndPlayer.Values.All(p => p.isReady);
        }

        private static bool DeviceIsPaired(InputDevice device)
        {
            // for more, refer to https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/HowDoI.html
            return (PlayerInput.FindFirstPairedToDevice(device) != null);
        }

        private bool HasVacancy()
        {
            return indexAndPlayer.Count < maxPlayerCount;
        }

        public static bool CurrentDeviceIsPaired()
        {
            if (DeviceIsPaired(Keyboard.current)) return true;
            if (DeviceIsPaired(Gamepad.current)) return true;
            return false;
        }
    }
}