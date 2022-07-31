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
        public CharacterInGameController inGameController; // only X movement
        public CharacterInXzGameController inXzGameController; // XZ movement
        public EndingScreenController endingScreenController; // singleton, every player shares the same object

        public static PlayerControlObjectData Create()
        {
            return new PlayerControlObjectData();
        }

        public void SetControlObjectActiveState(int playerId,
                                                GameControlState controlState,
                                                GameObject characterInGamePrefab,
                                                GameObject characterInXzGamePrefab,
                                                GameObject characterInLobbyPrefab,
                                                GameScene gameScene,
                                                SceneInitData sceneInitData)
        {
            if (controlState == GameControlState.TITLE_SCREEN_MENU)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                UnityFn.DestroyReferenceIfPresent(inXzGameController, () => inXzGameController = null);
                UnityFn.DestroyReferenceIfPresent(endingScreenController, () => endingScreenController = null);
            }
            if (controlState == GameControlState.TITLE_SCREEN_LOBBY)
            {
                if (inLobbyController == null) inLobbyController = UnityFn.InstantiateCharacterObject<CharacterInLobbyController>(characterInLobbyPrefab, true, Vector3.zero);
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                UnityFn.DestroyReferenceIfPresent(inXzGameController, () => inXzGameController = null);
                UnityFn.DestroyReferenceIfPresent(endingScreenController, () => endingScreenController = null);
            }
            if (controlState == GameControlState.INFO_SCREEN)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                if (infoScreenCanvasController == null) infoScreenCanvasController = InfoScreenCanvasController.GetInstance();
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                UnityFn.DestroyReferenceIfPresent(inXzGameController, () => inXzGameController = null);
                UnityFn.DestroyReferenceIfPresent(endingScreenController, () => endingScreenController = null);
            }
            if (controlState == GameControlState.IN_GAME && !gameScene.moveXZ)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                if (inGameController == null) inGameController = UnityFn.InstantiateCharacterObject<CharacterInGameController>(characterInGamePrefab, false, sceneInitData.GetRandomPlayerInitPosition()).Init(playerId, true);
                UnityFn.DestroyReferenceIfPresent(inXzGameController, () => inXzGameController = null);
                UnityFn.DestroyReferenceIfPresent(endingScreenController, () => endingScreenController = null);
            }
            if (controlState == GameControlState.IN_GAME && gameScene.moveXZ)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                if (inXzGameController == null) inXzGameController = UnityFn.InstantiateCharacterObject<CharacterInXzGameController>(characterInXzGamePrefab, false, sceneInitData.GetRandomPlayerInitPosition()).Init(playerId, true);
                UnityFn.DestroyReferenceIfPresent(endingScreenController, () => endingScreenController = null);
            }
            if (controlState == GameControlState.ENDING_SCREEN)
            {
                UnityFn.DestroyReferenceIfPresent(inLobbyController, () => inLobbyController = null);
                UnityFn.DestroyReferenceIfPresent(infoScreenCanvasController, () => infoScreenCanvasController = null);
                UnityFn.DestroyReferenceIfPresent(inGameController, () => inGameController = null);
                UnityFn.DestroyReferenceIfPresent(inXzGameController, () => inXzGameController = null);
                if (endingScreenController == null) endingScreenController = EndingScreenController.GetInstance();
            }
        }
    }
}