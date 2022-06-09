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

        private EnemyBossLv1WeakPointController weakPointCtrl;
        private Animator animatorCtrl;
        private static readonly int isActive = Animator.StringToHash("isActive");
        private AppMusic musicController;

        private int phase = 0;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            guns = gameObject.GetComponentsInChildren<EnemyBossLv1GunController>();
            weakPointCtrl = weakPoint.GetComponent<EnemyBossLv1WeakPointController>();
            animatorCtrl = gameObject.GetComponent<Animator>();
            SetGunsActive(false);
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
            SetGunsActive(true);
            animatorCtrl.SetBool(isActive, true);
            bossCamera.SetActive(true);
            gameCamera.SetActive(false);
            phase = 1;
        }

        private void HandlePhase1()
        {
            int brokenGuns = Fn.Filter(g => g.isBroken, new List<EnemyBossLv1GunController>(guns)).Count;
            if (brokenGuns == guns.Length)
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
                UnityFn.SetTimeout(AppResource.instance, 5, () =>
                {
                    UnityFn.LoadNextScene();
                    Destroy(gameObject);
                });
            }
        }

        private void SetGunsActive(bool isActive)
        {
            foreach (EnemyBossLv1GunController gun in guns)
            {
                gun.gameObject.SetActive(isActive);
            }
        }

        public override float GetDetectionRange()
        {
            return 50f;
        }
    }
}