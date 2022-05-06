using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player.Domain;
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

        public GameObject playerReadyText;

        private void Awake()
        {
            UnityFn.KeepAlive(gameObject);
        }

        public void Init(int id)
        {
            playerId = id;
            isInitialized = true;
            storeData = AppResource.instance.storeData;
            GameObject panel = ResourceTitleScene.instance.lobbyCharacterPanels[id];

            Transform gameObjectTransform = gameObject.transform;
            gameObjectTransform.localScale = new Vector3(10f, 10f, 10f);
            gameObjectTransform.position = panel.transform.position;
        }

        public void HandleUpdate(int id, UserInput userInput, List<GameObject> playerGameObjects)
        {
            if (!isInitialized) Init(id);
            gameObject.SetActive(true);
            if (userInput.left || userInput.right) Move(userInput);
            if (userInput.fire1) OkOrFire();
            if (userInput.jump || userInput.escape) Cancel(playerGameObjects);
        }

        public void Move(UserInput userInput)
        {
            // playerReadyText.SetActive(false);
            UpdateSkin(userInput);
        }

        public void OkOrFire()
        {
            // playerReadyText.SetActive(true);
            SetPlayerReady();
        }


        public void Cancel(List<GameObject> playerGameObjects)
        {
            // This can only call static method, because it destroys this object, so it needs to make a clean call
            // if the call target relies on this gameObject to exist, an error can occur
            playerGameObjects.ForEach(Destroy);
        }

        public void SetPlayerReady()
        {
            PlayerInputManagerData inputManagerData = storeData.inputManagerData;
            inputManagerData.SetPlayerReady(playerId, true);
            if (inputManagerData.AllPlayersAreReady()) OnSelectedStartFromLobby();
        }

        public void OnSelectedStartFromLobby()
        {
            // 1 second to allow sound effect
            UnityFn.SetTimeout(this, 1f, UnityFn.LoadNextScene);
        }

        public void UpdateSkin(UserInput userInput)
        {
            PlayerInputManagerData inputManagerData = storeData.inputManagerData;
            inputManagerData.SetPlayerReady(playerId, false);

            if (userInput.right)
            {
                int nextIndex = (currentSkinIndex + 1) % 3;
                ChangeSkin(nextIndex);
            }
            if (userInput.left)
            {
                int nextIndex = ((currentSkinIndex + 30) - 1) % 3;
                ChangeSkin(nextIndex);
            }
        }

        private void ChangeSkin(int desireIndex)
        {
            if (!canUpdateSkin) return;
            // Dictionary<string, RuntimeAnimatorController> skin = ResourceApp.instance.nameAndSkin.ElementAt(desireIndex).Value;
            // spriteInGame.GetComponent<Animator>().runtimeAnimatorController = skin["inGame"];
            // spriteInPanel.GetComponent<Animator>().runtimeAnimatorController = skin["panel"];
            Debug.Log("Changed skin to " + desireIndex);
            currentSkinIndex = desireIndex;
            canUpdateSkin = false;
            UnityFn.SetTimeout(this, 0.2f, () => canUpdateSkin = true);
        }
    }
}