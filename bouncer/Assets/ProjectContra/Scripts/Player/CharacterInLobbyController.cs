using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using BaseUtil.GameUtil.PlayerManagement;
using ProjectContra.Scripts.Player.Domain;
using ProjectContra.Scripts.Util;
using UnityEngine;

namespace ProjectContra.Scripts.Player
{
    public class CharacterInLobbyController : MonoBehaviour
    {
        private GameStoreData storeData;
        private bool isInitialized = false;
        private int playerId;
        public int currentSkinIndex = 0;
        private bool canUpdateSkin = true;
        private MeshRenderer meshRenderer;

        public GameObject playerReadyState;

        public void Init(int id)
        {
            playerId = id;
            isInitialized = true;
            storeData = AppResource.instance.storeData;
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = AppResource.instance.GetSkin(playerAttribute.skinId);
            GameObject panel = ResourceTitleScene.instance.lobbyCharacterPanels[id];

            Transform gameObjectTransform = gameObject.transform;
            gameObjectTransform.localScale = new Vector3(15f, 15f, 15f);
            gameObjectTransform.position = panel.transform.position;
        }

        public void HandleUpdate(int id, UserInput userInput, GameObject playerGameObject)
        {
            if (!isInitialized) Init(id);
            UnityFn.SetActive(gameObject);
            if (storeData.inputManagerData.AllPlayersAreReady()) return; // to avoid causing strange behaviour, when everyone is ready, stop further changes
            if (userInput.left || userInput.right) Move(userInput);
            if (userInput.fire1 || userInput.space) Ok();
            if (userInput.jump || userInput.escape) Cancel(playerGameObject);
        }

        public void Move(UserInput userInput)
        {
            SetPlayerNotReady();
            UpdateSkin(userInput);
        }

        public void Ok()
        {
            SetPlayerReady();
        }

        public void Cancel(GameObject playerGameObject)
        {
            bool isPlayerReady = storeData.inputManagerData.IsPlayerReady(playerId);
            if (isPlayerReady)
            {
                SetPlayerNotReady();
            }
            else
            {
                Destroy(playerGameObject); // destroying the object with PlayerInput forces the player to quit 
                Destroy(gameObject); // The game object controlled by Player is a separate object so needs to be removed as well
            }
        }

        public void SetPlayerReady()
        {
            playerReadyState.SetActive(true);
            PlayerInputManagerData inputManagerData = storeData.inputManagerData;
            inputManagerData.SetPlayerReady(playerId, true);
            if (inputManagerData.AllPlayersAreReady()) OnSelectedStartFromLobby();
        }

        public void SetPlayerNotReady()
        {
            playerReadyState.SetActive(false);
            PlayerInputManagerData inputManagerData = storeData.inputManagerData;
            inputManagerData.SetPlayerReady(playerId, false);
        }

        public void OnSelectedStartFromLobby()
        {
            // 1 second to allow sound effect
            UnityFn.SetTimeout(this, 1f, () => SceneUtil.LoadNextScene(AppResource.instance));
        }

        public void UpdateSkin(UserInput userInput)
        {
            PlayerInputManagerData inputManagerData = storeData.inputManagerData;
            inputManagerData.SetPlayerReady(playerId, false);
            int skinCount = AppResource.instance.GetSkinCount();
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);

            if (userInput.right)
            {
                int nextIndex = FnVal.GetNextIndex(playerAttribute.skinId, skinCount);
                ChangeSkin(nextIndex);
            }
            if (userInput.left)
            {
                int nextIndex = FnVal.GetPreviousIndex(playerAttribute.skinId, skinCount);
                ChangeSkin(nextIndex);
            }
        }

        private void ChangeSkin(int desireIndex)
        {
            UnityFn.RunWithInterval(this, 0.2f, canUpdateSkin, b => canUpdateSkin = b, () =>
            {
                PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
                meshRenderer.material = AppResource.instance.GetSkin(desireIndex);
                playerAttribute.skinId = desireIndex;
                storeData.SetPlayer(playerAttribute);
            });
        }
    }
}