using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using ProjectContra.Scripts.Util;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv1Controller : AbstractRangeDetectionController
    {
        private GameStoreData storeData;
        private EnemyBossLv1GunController[] guns;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;

        [SerializeField] private GameObject weakPoint;
        [SerializeField] private GameObject spawnPoint;

        private EnemyBossWeakPointController weakPointCtrl;
        private Animator animatorCtrl;
        private static readonly int isActive = Animator.StringToHash("isActive");
        private AppMusic musicController;

        private int phase = 0;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            guns = gameObject.GetComponentsInChildren<EnemyBossLv1GunController>();
            weakPointCtrl = weakPoint.GetComponent<EnemyBossWeakPointController>();
            animatorCtrl = gameObject.GetComponent<Animator>();
            UnityFn.SetControllersActive(guns, false);
            weakPoint.SetActive(false);
            spawnPoint.SetActive(false);
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                if (phase == 0) HandlePhase0();
                if (phase == 1) HandlePhase1();
                if (phase == 2) HandlePhase2();
            });
        }

        private void HandlePhase0()
        {
            musicController.PlayLv1BossMusic();
            UnityFn.SetControllersActive(guns, true);
            animatorCtrl.SetBool(isActive, true);
            bossCamera.SetActive(true);
            gameCamera.SetActive(false);
            phase = 1;
        }

        private void HandlePhase1()
        {
            bool areAllBroken = AbstractDestructibleController.AreAllBroken(guns);
            if (areAllBroken)
            {
                weakPoint.SetActive(true);
                spawnPoint.SetActive(true);
                phase = 2;
            }
        }

        private void HandlePhase2()
        {
            if (weakPointCtrl.isBroken)
            {
                phase = 3; // there is no phase 3, this is just to prevent getting into here again
                spawnPoint.SetActive(false);
                gameCamera.SetActive(true);
                bossCamera.SetActive(false);
                AppMusic.instance.Stop();
                EnemyWalkerController[] mods = FindObjectsOfType<EnemyWalkerController>();
                AbstractDestructibleController.KillAll(mods);
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
            return 50f;
        }
    }
}