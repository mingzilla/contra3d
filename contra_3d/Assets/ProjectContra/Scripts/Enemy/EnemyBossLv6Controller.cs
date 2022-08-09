using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Util;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv6Controller : AbstractRangeDetectionController
    {
        private GameStoreData storeData;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;

        [SerializeField] private int detectionRange = 50;
        [SerializeField] private GameObject dragonHead;
        [SerializeField] private GameObject weakPoint;
        [SerializeField] private GameObject[] dragonEyes;
        [SerializeField] private GameObject[] spiderSpawnPoints;
        [SerializeField] private Vector3 dragonHeadPositionDelta;

        private EnemyWalkingShooterController[] movingHeadControllers;
        private EnemyShooterController[] dragonEyeControllers;
        private EnemyBossWeakPointController weakPointController;
        private Vector3 dragonHeadTargetPosition;
        private AppMusic musicController;
        private int phase = 0;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();

            // phase 1
            movingHeadControllers = GetComponentsInChildren<EnemyWalkingShooterController>();
            UnityFn.SetControllersActive(movingHeadControllers, false);

            // phase 2
            weakPointController = weakPoint.GetComponent<EnemyBossWeakPointController>();
            weakPoint.SetActive(false);
            dragonHead.SetActive(false);
            dragonHeadTargetPosition = dragonHead.transform.position + dragonHeadPositionDelta;
            dragonEyeControllers = Fn.MapArray(x => x.GetComponent<EnemyShooterController>(), dragonEyes);
            UnityFn.SetControllersActive(dragonEyeControllers, false);
            UnityFn.SetAllInactivate(new List<GameObject>(spiderSpawnPoints));
        }

        void Update()
        {
            if (phase == 0) TriggerIfPlayerIsInRange(storeData, GetDetectionRange(), HandlePhase0ActivateBoss);
            if (phase == 1) HandlePhase1FightingBoss1();
            if (phase == 2) HandlePhase2MovingBoss2();
            if (phase == 3) HandlePhase3ActivateBoss2();
            if (phase == 4) HandlePhase4FightingBoss2();
            if (phase == 5) HandlePhase5();
        }

        private void HandlePhase0ActivateBoss(Transform closestPlayer)
        {
            musicController.PlayLv6BossMusic1();
            bossCamera.SetActive(true);
            gameCamera.SetActive(false);
            UnityFn.SetControllersActive(movingHeadControllers, true);
            phase = 1;
        }

        private void HandlePhase1FightingBoss1()
        {
            bool areAllBroken = AbstractDestructibleController.AreAllBroken(movingHeadControllers);
            if (areAllBroken)
            {
                musicController.Stop();
                phase = 2;
                UnityFn.SetTimeout(this, 3f, () =>
                {
                    AppSfx.PlayRepeatedly(AppSfx.instance.floorMove, 5);
                });
            }
        }

        private void HandlePhase2MovingBoss2()
        {
            UnityFn.FastSetActive(dragonHead, true);
            bool isMoving = MovementUtil.MoveToPositionOverTime(dragonHead.transform, dragonHeadTargetPosition, 0.1f, 4);
            if (!isMoving) phase = 3;
        }

        private void HandlePhase3ActivateBoss2()
        {
            musicController.PlayLv6BossMusic2();
            weakPoint.SetActive(true);
            UnityFn.SetControllersActive(dragonEyeControllers, true);
            UnityFn.SetAllActivate(new List<GameObject>(spiderSpawnPoints));
            phase = 4;
        }

        private void HandlePhase4FightingBoss2()
        {
            if (weakPointController.isBroken) phase = 5;
        }

        private void HandlePhase5()
        {
            phase = 6; // this is just to prevent getting into here again
            gameCamera.SetActive(true);
            bossCamera.SetActive(false);
            UnityFn.SetAllInactivate(new List<GameObject>(spiderSpawnPoints));
            UnityFn.SetControllersActive(dragonEyeControllers, false);
            AppMusic.instance.Stop();
            UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 5f);
            EnemyWalkingShooterController[] mods = FindObjectsOfType<EnemyWalkingShooterController>();
            AbstractDestructibleController.KillAll(mods);
            UnityFn.SetTimeout(AppResource.instance, 5, () =>
            {
                AppSfx.instance.levelClear.Play();
                UnityFn.SetTimeout(AppResource.instance, 5, GameFn.LoadNextScene);
                Destroy(gameObject);
            });
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}