using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Player.Domain
{
    public class PlayerControlObjectData
    {
        public CharacterInLobbyController inLobbyController;
        public InfoScreenCanvasController infoScreenCanvasController; // singleton, every player shares the same object
        public CharacterInGameController inGameController;

        public static PlayerControlObjectData Create()
        {
            return new PlayerControlObjectData();
        }

        public void SetControlObjectActiveState(int playerId, GameControlState controlState, GameObject characterInGamePrefab, GameObject characterInLobbyPrefab)
        {
            if (controlState == GameControlState.TITLE_SCREEN_LOBBY)
            {
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                if (inLobbyController == null) inLobbyController = UnityFn.InstantiateCharacterObject<CharacterInLobbyController>(characterInLobbyPrefab, true);
            }
            if (controlState == GameControlState.INFO_SCREEN)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                if (infoScreenCanvasController == null) infoScreenCanvasController = InfoScreenCanvasController.GetInstance();
            }
            if (controlState == GameControlState.IN_GAME)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                if (inGameController == null)
                {
                    inGameController = UnityFn.InstantiateCharacterObject<CharacterInGameController>(characterInGamePrefab, false);
                    inGameController.Init(playerId, true);
                }
            }
        }

        public List<GameObject> GetControlObjects()
        {
            return Fn.WithoutNull(new List<GameObject>()
            {
                (inLobbyController != null ? inLobbyController.gameObject : null),
                (inGameController != null ? inGameController.gameObject : null),
                AppResource.instance.infoScreenPrefab
            });
        }
    }
}