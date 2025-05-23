﻿using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Util;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv3Controller : AbstractRangeDetectionController
    {
        private GameStoreData storeData;
        private EnemyBossLv3GunController[] guns;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;
        [SerializeField] private float detectionRange = 50f;

        private EnemyBossWeakPointController weakPointCtrl;
        private Animator animatorCtrl;
        private static readonly int isActiveKey = Animator.StringToHash("isActive");
        private AppMusic musicController;

        private int phase = 0;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            guns = gameObject.GetComponentsInChildren<EnemyBossLv3GunController>();
            weakPointCtrl = gameObject.GetComponentInChildren<EnemyBossWeakPointController>();
            UnityFn.SetControllerActive(weakPointCtrl, false);
            animatorCtrl = gameObject.GetComponent<Animator>();
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
            musicController.PlayLv3BossMusic();
            animatorCtrl.SetBool(isActiveKey, true);
            bossCamera.SetActive(true);
            gameCamera.SetActive(false);
            UnityFn.SetControllerActive(weakPointCtrl, true);
            UnityFn.SetTimeout(this, 2, () =>
            {
                Fn.EachInArray(x => x.canShoot = true, guns);
            });
            phase = 1;
        }

        private void HandlePhase1()
        {
            if (weakPointCtrl.isBroken)
            {
                phase = 2; // this is just to prevent getting into here again
                Fn.EachInArray(x => x.Explode(), guns);
                gameCamera.SetActive(true);
                bossCamera.SetActive(false);
                AppMusic.instance.Stop();
                UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 5f);
                GameFn.LoadNextSceneAfterBossKilled(gameObject, false);
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}