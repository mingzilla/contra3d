using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BaseUtil.GameUtil.PlayerManagement
{
    public class PlayerInputManagerData
    {
        private readonly Dictionary<int, PlayerInputAndStatus> indexAndPlayer = new Dictionary<int, PlayerInputAndStatus>();
        private int maxPlayerCount = 4;

        public static PlayerInputManagerData Create(int maxPlayerCount)
        {
            return new PlayerInputManagerData()
            {
                maxPlayerCount = maxPlayerCount
            };
        }

        public void AddPlayer(PlayerInput playerInput)
        {
            indexAndPlayer[playerInput.playerIndex] = PlayerInputAndStatus.Create(playerInput);
        }

        public PlayerInput GetPlayer(int index)
        {
            return indexAndPlayer[index].playerInput;
        }

        public bool IsPlayerReady(int index)
        {
            return indexAndPlayer[index].isReady;
        }

        public void RemovePlayer(PlayerInput playerInput)
        {
            indexAndPlayer.Remove(playerInput.playerIndex);
        }

        public void RemovePlayerByIndex(int index)
        {
            indexAndPlayer.Remove(index);
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

        public bool CanJoinPlayer(bool isInLobby, InputDevice device)
        {
            return isInLobby &&
                   HasVacancy() &&
                   !AllPlayersAreReady() && // if everyone is ready, stop allowing people jumping in with an !isReady state
                   !DeviceIsPaired(device);
        }

        /// <summary>
        /// Allow getting a low value slot, if 4 are allowed and index 0 is available, 0 is returned
        /// </summary>
        public int GetNextIndexToJoin()
        {
            for (int i = 0; i < maxPlayerCount; i++)
            {
                if (!indexAndPlayer.ContainsKey(i)) return i;
            }
            return (maxPlayerCount - 1);
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
            return indexAndPlayer.Count > 0 && // if no players, nobody is ready
                   indexAndPlayer.Values.All(p => p.isReady);
        }

        public List<PlayerInputAndStatus> AllPlayers()
        {
            return indexAndPlayer.Values.ToList();
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