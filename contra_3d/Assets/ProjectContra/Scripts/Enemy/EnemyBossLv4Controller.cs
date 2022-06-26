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
    public class EnemyBossLv4Controller : AbstractRangeDetectionController
    {
        private GameStoreData storeData;
        private EnemyBossLv4GunController[] guns;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;
        [SerializeField] private float detectionRange = 50f;

        private Animator animatorCtrl;
        private static readonly int isActiveKey = Animator.StringToHash("isActive");
        private AppMusic musicController;

        private int phase = 0;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            guns = gameObject.GetComponentsInChildren<EnemyBossLv4GunController>();
            animatorCtrl = gameObject.GetComponent<Animator>();
            animatorCtrl.enabled = false;
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
            int brokenGuns = Fn.Filter(g => g.isBroken, new List<EnemyBossLv4GunController>(guns)).Count;
            if (brokenGuns == guns.Length)
            {
                phase = 2; // this is just to prevent getting into here again
                gameCamera.SetActive(true);
                bossCamera.SetActive(false);
                AppMusic.instance.Stop();
                UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 5f);
                UnityFn.SetTimeout(AppResource.instance, 5, () =>
                {
                    AppSfx.instance.levelClear.Play();
                    UnityFn.SetTimeout(AppResource.instance, 5, UnityFn.LoadNextScene);
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