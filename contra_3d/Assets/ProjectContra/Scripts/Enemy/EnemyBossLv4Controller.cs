using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Map;
using ProjectContra.Scripts.Util;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv4Controller : MonoBehaviour
    {
        private GameStoreData storeData;
        private EnemyBossLv4GunController[] guns;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;
        [SerializeField] private GameObject bossTrigger;

        private Animator animatorCtrl;
        private TriggerByAnyPlayerEnterController bossTriggerCtrl;
        private AppMusic musicController;

        private int phase = 0;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            bossTriggerCtrl = bossTrigger.GetComponent<TriggerByAnyPlayerEnterController>();
            guns = gameObject.GetComponentsInChildren<EnemyBossLv4GunController>();
            animatorCtrl = gameObject.GetComponent<Animator>();
            animatorCtrl.enabled = false;
        }

        void Update()
        {
            if (!bossTriggerCtrl.isActivated) return;
            if (phase == 0) HandlePhase0();
            if (phase == 1) HandlePhase1();
        }

        private void HandlePhase0()
        {
            musicController.PlayLv4BossMusic();
            animatorCtrl.enabled = true;
            bossCamera.SetActive(true);
            gameCamera.SetActive(false);
            UnityFn.SetTimeout(AppResource.instance, 2, () =>
            {
                Fn.EachInArray(x => x.canShoot = true, guns);
            });
            phase = 1;
        }

        private void HandlePhase1()
        {
            bool areAllBroken = AbstractDestructibleController.AreAllBroken(guns);
            if (areAllBroken)
            {
                phase = 2; // this is just to prevent getting into here again
                gameCamera.SetActive(true);
                bossCamera.SetActive(false);
                animatorCtrl.enabled = false;
                AppMusic.instance.Stop();
                UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 5f);
                UnityFn.SetTimeout(AppResource.instance, 5, () =>
                {
                    AppSfx.instance.levelClear.Play();
                    UnityFn.SetTimeout(AppResource.instance, 5, GameFn.LoadNextScene);
                    Destroy(gameObject);
                });
            }
        }
    }
}