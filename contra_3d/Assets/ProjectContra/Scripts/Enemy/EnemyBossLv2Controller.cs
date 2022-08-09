using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using ProjectContra.Scripts.Util;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv2Controller : AbstractRangeDetectionController
    {
        private GameStoreData storeData;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;
        private EnemyShooterController[] shooters;
        public float detectionRange = 40f;

        private Animator animatorCtrl;
        private AppMusic musicController;

        private int phase = 0;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            gameObject.layer = GameLayer.INVISIBLE_WALL_TO_PLAYER.GetLayer();
            animatorCtrl = gameObject.GetComponent<Animator>();
            animatorCtrl.enabled = false;
            shooters = gameObject.GetComponentsInChildren<EnemyShooterController>();
            UnityFn.SetControllersActive(shooters, false);
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                if (phase == 0) HandlePhase0();
                if (phase == 1) HandlePhase1();
            });
        }

        private void HandlePhase0()
        {
            musicController.PlayLv2BossMusic();
            bossCamera.SetActive(true);
            gameCamera.SetActive(false);
            UnityFn.SetControllersActive(shooters, true);
            animatorCtrl.enabled = true;
            phase = 1;
        }

        private void HandlePhase1()
        {
            int deadShooters = Fn.Filter(g => g.hp <= 0, new List<EnemyShooterController>(shooters)).Count;
            if (deadShooters == shooters.Length)
            {
                phase = 2; // there is no phase 3, this is just to prevent getting into here again
                gameCamera.SetActive(true);
                bossCamera.SetActive(false);
                animatorCtrl.enabled = false;
                AppMusic.instance.Stop();
                AppSfx.PlayRepeatedly(AppSfx.instance.bossDeath, 5);
                UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 5f);
                UnityFn.SetTimeout(AppResource.instance, 5, () =>
                {
                    AppSfx.instance.levelClear.Play();
                    UnityFn.SetTimeout(AppResource.instance, 5, GameFn.LoadNextScene);
                    Destroy(gameObject);
                });
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}