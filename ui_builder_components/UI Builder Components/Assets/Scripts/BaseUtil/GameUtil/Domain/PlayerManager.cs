using System;
using System.Collections.Generic;
using BaseUtil.Base;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BaseUtil.GameUtil.Domain
{
    public class PlayerManager<T> where T : MonoBehaviour
    {
        public int maxPlayerCount;
        public Dictionary<int, T> activePlayers;

        public int GetNextPlayerId()
        {
            return activePlayers.Count;
        }

        public static PlayerManager<T> CreateEmpty(int maxPlayerCount)
        {
            return new PlayerManager<T>
            {
                maxPlayerCount = maxPlayerCount,
                activePlayers = new Dictionary<int, T>(),
            };
        }

        public void AddPlayer(int id, T playerController, Action successFn)
        {
            if (activePlayers.Count < maxPlayerCount)
            {
                activePlayers = Fn.SetDictionary(activePlayers, id, playerController);
                successFn();
            }
            else
            {
                Object.Destroy(playerController.gameObject);
            }
        }

        public void SetPlayer(int id, T playerAttribute)
        {
            activePlayers = Fn.ModifyDictionary(activePlayers, id, playerAttribute);
        }

        public T GetPlayer(int id)
        {
            return activePlayers[id];
        }
    }
}