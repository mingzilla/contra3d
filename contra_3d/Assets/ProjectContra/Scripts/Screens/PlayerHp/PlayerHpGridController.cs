using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
using UnityEngine;

namespace ProjectContra.Scripts.Screens.PlayerHp
{
    public class PlayerHpGridController : MonoBehaviour
    {
        private GameStoreData storeData;
        [SerializeField] List<GameObject> playerHpIndicators;

        private void Update()
        {
            storeData = AppResource.instance.storeData;
            storeData.AllPlayerIds().ForEach(playerId =>
            {
                PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
                GameObject playerHpIndicator = playerHpIndicators[playerId];
                bool isPresent = playerAttribute != null && playerHpIndicator != null;
                UnityFn.FastSetActive(playerHpIndicator, isPresent);
            });
        }
    }
}