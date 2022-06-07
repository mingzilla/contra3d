using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectContra.Scripts.Screens.PlayerHp
{
    public class PlayerHpUiController : MonoBehaviour
    {
        [SerializeField] int playerId = 0;
        private GameStoreData storeData;
        private Image image;

        private void Start()
        {
            image = gameObject.GetComponent<Image>();
            gameObject.SetActive(false);
            storeData = AppResource.instance.storeData;
        }

        void Update()
        {
            PlayerAttribute playerAttribute = storeData.GetPlayer(playerId);
            bool isPresent = playerAttribute != null && image != null;
            if (isPresent)
            {
                image.fillAmount = ((float) playerAttribute.currentHp) / playerAttribute.maxHp;
            }
        }
    }
}