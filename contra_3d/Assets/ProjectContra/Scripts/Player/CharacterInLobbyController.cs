using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using BaseUtil.GameUtil.PlayerManagement;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
using ProjectContra.Scripts.Screens;
using ProjectContra.Scripts.Util;
using UnityEngine;

namespace ProjectContra.Scripts.Player
{
    public class CharacterInLobbyController : MonoBehaviour
    {
        [SerializeField] private GameObject meshObject;
        [SerializeField] private int[] xPositionDelta = new[] {6, 1, -1, -6};

        private GameStoreData storeData;
        private bool isInitialized = false;
        private int playerId;

        private readonly IntervalState buttonIntervalState = IntervalState.Create(0.1f);
        private MeshRenderer meshRenderer;
        private GameObject placeholderPanel;
        private LobbyPlayerPlaceholderController placeholderPanelCtrl;

        public void Init(int id)
        {
            playerId = id;
            isInitialized = true;
            storeData = AppResource.instance.storeData;
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
            meshRenderer = meshObject.GetComponent<MeshRenderer>();
            Material skin = AppResource.instance.GetSkin(playerAttribute.skinId);
            meshRenderer.materials = UnityFn.UpdateMaterialAt(meshRenderer.materials, 1, skin);
            placeholderPanel = ResourceTitleScene.instance.lobbyCharacterPanels[id];
            placeholderPanelCtrl = placeholderPanel.GetComponent<LobbyPlayerPlaceholderController>();
            placeholderPanelCtrl.SetPlayerJoinedStatus();
            placeholderPanelCtrl.SetPlayerReadyFalse();

            Transform gameObjectTransform = gameObject.transform;
            gameObjectTransform.position = placeholderPanel.transform.position + new Vector3(xPositionDelta[playerId], 0, 0);
        }

        public void HandleUpdate(int id, UserInput userInput, GameObject playerGameObject)
        {
            if (!isInitialized) Init(id);
            UnityFn.SetActive(gameObject);
            if (storeData.inputManagerData.AllPlayersAreReady()) return; // to avoid causing strange behaviour, when everyone is ready, stop further changes
            if (userInput.left || userInput.right) Move(userInput);
            if (userInput.fire1) Ok();
            if (userInput.jump) Cancel(playerGameObject);
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
                placeholderPanelCtrl.SetPlayerLeftStatus();
                Destroy(playerGameObject); // destroying the object with PlayerInput forces the player to quit 
                Destroy(gameObject); // The game object controlled by Player is a separate object so needs to be removed as well
                if (storeData.inputManagerData.AllPlayersAreReady()) StartGame(); // if p1 is ready, p2 quit, p1 should start game
            }
        }

        public void SetPlayerReady()
        {
            placeholderPanelCtrl.SetPlayerReadyTrue();
            PlayerInputManagerData inputManagerData = storeData.inputManagerData;
            inputManagerData.SetPlayerReady(playerId, true);
            if (inputManagerData.AllPlayersAreReady()) StartGame();
        }

        public void SetPlayerNotReady()
        {
            placeholderPanelCtrl.SetPlayerReadyFalse();
            PlayerInputManagerData inputManagerData = storeData.inputManagerData;
            inputManagerData.SetPlayerReady(playerId, false);
        }

        public void StartGame()
        {
            UnityFn.RunWithInterval(AppResource.instance, buttonIntervalState, GameFn.LoadNextScene);
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
            UnityFn.RunWithInterval(this, buttonIntervalState, () =>
            {
                PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
                playerAttribute.skinId = desireIndex;
                Material skin = AppResource.instance.GetSkin(playerAttribute.skinId);
                meshRenderer.materials = UnityFn.UpdateMaterialAt(meshRenderer.materials, 1, skin);
                storeData.SetPlayer(playerAttribute);
            });
        }
    }
}