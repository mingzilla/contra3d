using System.Collections.Generic;
using System.Linq;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.PlayerManagement;
using ProjectContent.Scripts.Types;
using UnityEngine;
using UnityEngine.InputSystem;
using ProjectContent.Scripts.Player;

namespace ProjectContent.Scripts.Data
{
    public class GameStoreData
    {
        private Dictionary<int, PlayerAttribute> idAndPlayerState = new Dictionary<int, PlayerAttribute>();
        public readonly PlayerInputManagerData inputManagerData = PlayerInputManagerData.Create(4);
        public GameInputContext controlState = GameInputContext.GAME_PLAY;

        public GameStoreData Init()
        {
            return this;
        }

        public int GetNextPlayerId()
        {
            return inputManagerData.GetNextIndexToJoin();
        }

        public bool IsFirstPlayer(int playerId)
        {
            return playerId == AllPlayerIds().Min();
        }

        public void SetPlayer(PlayerAttribute playerAttribute)
        {
            idAndPlayerState = Fn.ModifyDictionary(idAndPlayerState, playerAttribute.playerId, playerAttribute);
        }

        public PlayerAttribute GetPlayer(int id)
        {
            if (!idAndPlayerState.ContainsKey(id)) return null;
            return idAndPlayerState[id];
        }

        public void AddPlayer(PlayerInput playerInput)
        {
            inputManagerData.AddPlayer(playerInput);
            SetPlayer(PlayerAttribute.CreateEmpty(playerInput.playerIndex));
        }

        public void RemovePlayer(int id)
        {
            inputManagerData.RemovePlayerByIndex(id);
            idAndPlayerState.Remove(id);
        }

        public List<int> AllPlayerIds()
        {
            return idAndPlayerState.Keys.ToList();
        }

        public bool AllPlayersDead()
        {
            return idAndPlayerState.Values.All(p => !p.isAlive);
        }

        public PlayerMono GetPlayerMono(int id)
        {
            PlayerInput playerInput = inputManagerData.GetPlayer(id);
            return playerInput.gameObject.GetComponent<PlayerMono>();
        }

        public List<PlayerMono> GetAllPlayerMonos()
        {
            return Fn.Map(p => GetPlayerMono(p.playerInput.playerIndex), inputManagerData.AllPlayers());
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

        public List<PlayerAttribute> GetVisiblePlayers()
        {
            // a player may not have a position, especially when they disappear or die
            return Fn.Filter(x => x.inGameTransform != null && x.isAlive, idAndPlayerState.Values.ToList());
        }

        public void ResetPlayers()
        {
            idAndPlayerState.Values.ToList().ForEach(x => x.Reset());
        }

        public void ReloadScene()
        {
            UnityFn.ReloadCurrentScene();
            ResetPlayers();
        }

        public bool IsPaused()
        {
            return controlState == GameInputContext.IN_GAME_MENU;
        }

        public bool IsInGame()
        {
            return controlState == GameInputContext.GAME_PLAY;
        }
    }
}