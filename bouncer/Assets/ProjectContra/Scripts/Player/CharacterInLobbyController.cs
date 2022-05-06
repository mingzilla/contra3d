using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player.Domain;
using ProjectContra.Scripts.Types;
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

        public void Init(int id)
        {
            playerId = id;
            isInitialized = true;
            storeData = AppResource.instance.storeData;
            GameObject panel = ResourceTitleScene.instance.lobbyCharacterPanels[id];

            Transform gameObjectTransform = gameObject.transform;
            gameObjectTransform.localScale = new Vector3(0.01f, 0.01f, 0);
            gameObjectTransform.position = panel.transform.position;
        }

        public void HandleUpdate(int id, UserInput userInput)
        {
            if (!isInitialized) Init(id);
            gameObject.SetActive(true);
            if (userInput.left || userInput.right) Move(userInput);
            if (userInput.fire1) OkOrFire();
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

        public void SetPlayerReady()
        {
            PlayerInputManagerData inputManagerData = storeData.inputManagerData;
            inputManagerData.SetPlayerReady(playerId, true);
            if (inputManagerData.AllPlayersAreReady()) OnSelectedStartFromLobby();
        }

        public void OnSelectedStartFromLobby()
        {
            UnityFn.LoadNextScene();
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