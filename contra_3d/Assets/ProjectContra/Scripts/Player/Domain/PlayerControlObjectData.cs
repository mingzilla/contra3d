using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.GameDataScriptable;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Player.Domain
{
    public class PlayerControlObjectData
    {
        public CharacterInLobbyController inLobbyController;
        public InfoScreenCanvasController infoScreenCanvasController; // singleton, every player shares the same object
        public CharacterInGameController inGameController;
        public PausedMenuController pausedMenuController; // singleton, every player shares the same object

        public static PlayerControlObjectData Create()
        {
            return new PlayerControlObjectData();
        }

        public void SetControlObjectActiveState(int playerId, GameControlState controlState, GameObject characterInGamePrefab, GameObject characterInLobbyPrefab, SceneInitData sceneInitData)
        {
            if (controlState == GameControlState.TITLE_SCREEN_MENU)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                UnityFn.DestroyReferenceIfPresent(pausedMenuController, () => pausedMenuController = null);
            }
            if (controlState == GameControlState.TITLE_SCREEN_LOBBY)
            {
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                UnityFn.DestroyReferenceIfPresent(pausedMenuController, () => pausedMenuController = null);
                if (inLobbyController == null) inLobbyController = UnityFn.InstantiateCharacterObject<CharacterInLobbyController>(characterInLobbyPrefab, true, Vector3.zero);
            }
            if (controlState == GameControlState.INFO_SCREEN)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                UnityFn.DestroyReferenceIfPresent(pausedMenuController, () => pausedMenuController = null);
                if (infoScreenCanvasController == null) infoScreenCanvasController = InfoScreenCanvasController.GetInstance();
            }
            if (controlState == GameControlState.IN_GAME)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                UnityFn.DestroyReferenceIfPresent(pausedMenuController, () => pausedMenuController = null);
                if (inGameController == null)
                {
                    inGameController = UnityFn.InstantiateCharacterObject<CharacterInGameController>(characterInGamePrefab, false, sceneInitData.playerInitPosition).Init(playerId, true);
                }
            }
            if (controlState == GameControlState.IN_GAME_PAUSED)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                // don't touch inGameController
                if (pausedMenuController == null) pausedMenuController = PausedMenuController.GetInstance();
            }
        }
    }
}