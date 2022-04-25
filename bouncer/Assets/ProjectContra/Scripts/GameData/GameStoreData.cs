using System.Collections.Generic;
using BaseUtil.Base;
using ProjectContra.Scripts.Player.domain;

namespace ProjectContra.Scripts.GameData
{
    public class GameStoreData
    {
        private Dictionary<int, PlayerAttribute> idAndPlayerState = new Dictionary<int, PlayerAttribute>();

        public void SetPlayer(PlayerAttribute playerAttribute)
        {
            idAndPlayerState = Fn.ModifyDictionary(idAndPlayerState, playerAttribute.playerId, playerAttribute);
        }

        public PlayerAttribute GetPlayer(int id)
        {
            return idAndPlayerState[id];
        }
    }
}