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
        public InfoScreenCanvasController infoScreenCanvasController;
        public CharacterInGameController inGameController;

        public static PlayerControlObjectData Create()
        {
            return new PlayerControlObjectData();
        }

        public void SetControlObjectActiveState(int playerId, GameControlState controlState, GameObject characterInGamePrefab, GameObject characterInLobbyPrefab)
        {
            if (controlState == GameControlState.TITLE_SCREEN_LOBBY) MaintainLobbyControlObject(characterInLobbyPrefab);
            if (controlState == GameControlState.INFO_SCREEN) MaintainInfoScreenControlObject();
            if (controlState == GameControlState.IN_GAME) MaintainInGameControlObject(playerId, characterInGamePrefab);
        }

        public void MaintainLobbyControlObject(GameObject prefab)
        {
            if (inLobbyController == null) inLobbyController = UnityFn.InstantiateDisabledCharacterObject<CharacterInLobbyController>(prefab);
            UnityFn.SetActiveAndDeActivateOthers(inLobbyController.gameObject, GetControlObjectsExcept(inLobbyController));
        }

        public void MaintainInfoScreenControlObject()
        {
            if (infoScreenCanvasController == null) infoScreenCanvasController = AppResource.instance.infoScreen.GetComponent<InfoScreenCanvasController>();
            UnityFn.SetActiveAndDeActivateOthers(AppResource.instance.infoScreen, GetControlObjectsExcept(infoScreenCanvasController));
        }

        public void MaintainInGameControlObject(int playerId, GameObject prefab)
        {
            if (inGameController == null)
            {
                inGameController = UnityFn.InstantiateDisabledCharacterObject<CharacterInGameController>(prefab);
                inGameController.Init(playerId);
            }
            UnityFn.SetActiveAndDeActivateOthers(inGameController.gameObject, GetControlObjectsExcept(inGameController));
        }

        private List<GameObject> GetControlObjectsExcept(MonoBehaviour exclude)
        {
            List<GameObject> list = GetControlObjects();
            list.Remove(exclude.gameObject);
            return list;
        }

        public List<GameObject> GetControlObjects()
        {
            return Fn.WithoutNull(new List<GameObject>()
            {
                (inLobbyController != null ? inLobbyController.gameObject : null),
                (inGameController != null ? inGameController.gameObject : null),
                AppResource.instance.infoScreen
            });
        }
    }
}