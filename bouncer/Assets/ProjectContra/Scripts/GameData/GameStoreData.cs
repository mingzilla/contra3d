using System.Collections.Generic;
using System.Linq;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Player.domain;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.GameData
{
    public class GameStoreData
    {
        private Dictionary<int, PlayerAttribute> idAndPlayerState = new Dictionary<int, PlayerAttribute>();
        public GameScene currentScene;
        public GameControlState controlState;

        public GameStoreData Init(GameScene activeScene)
        {
            currentScene = activeScene;
            controlState = currentScene.initialControlState;
            return this;
        }

        public int GetNextPlayerId()
        {
            return idAndPlayerState.Count;
        }

        public void SetPlayer(PlayerAttribute playerAttribute)
        {
            idAndPlayerState = Fn.ModifyDictionary(idAndPlayerState, playerAttribute.playerId, playerAttribute);
        }

        public PlayerAttribute GetPlayer(int id)
        {
            return idAndPlayerState[id];
        }

        public List<Vector3> AllPlayerPositions()
        {
            return Fn.Map(x => x.inGameTransform.position, GetVisiblePlayers());
        }

        public bool HasPlayer()
        {
            return GetVisiblePlayers().Count > 0;
        }

        public PlayerAttribute GetClosestPlayer(Vector3 position)
        {
            return UnityFn.GetClosestTarget(position, GetVisiblePlayers(), p => p.inGameTransform.position);
        }

        private List<PlayerAttribute> GetVisiblePlayers()
        {
            // a player may not have a position, especially when they disappear
            return Fn.Filter(x => x.inGameTransform != null && x.isAlive, idAndPlayerState.Values.ToList());
        }
    }
}