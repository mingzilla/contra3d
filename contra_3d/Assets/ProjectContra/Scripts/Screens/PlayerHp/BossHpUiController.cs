using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.GameData;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectContra.Scripts.Screens.PlayerHp
{
    public class BossHpUiController : MonoBehaviour
    {
        [SerializeField] private GameObject boss;

        public AbstractDestructibleController bossCtrl;
        private Image image;
        private GameStoreData storeData;

        private void Start()
        {
            image = gameObject.GetComponent<Image>();
            gameObject.SetActive(false);
            if (boss != null) bossCtrl = boss.GetComponent<AbstractDestructibleController>();
        }

        void Update()
        {
            if (!bossCtrl || !image) return;
            image.fillAmount = ((float) bossCtrl.hp) / bossCtrl.maxHp;
        }
    }
}