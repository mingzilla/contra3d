using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv8Controller : AbstractRangeDetectionController
    {
        private GameStoreData storeData;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;

        [SerializeField] private int detectionRange = 50;
        [SerializeField] private GameObject bossBody;
        [SerializeField] private Vector3 bossBodyPositionDelta = new Vector3(0, 25, 0);
        [SerializeField] private GameObject weakPoint;
        [SerializeField] private GameObject[] modSpawnPoints;
        [SerializeField] private GameObject[] laserShooters;

        private EnemyBossWeakPointController weakPointController;
        private AppMusic musicController;
        private Vector3 bossBodyHeadTargetPosition;
        private int phase = 0;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            weakPointController = weakPoint.GetComponent<EnemyBossWeakPointController>();
            bossBodyHeadTargetPosition = bossBody.transform.position + bossBodyPositionDelta;
            Deactivate();
        }

        private void Deactivate()
        {
            UnityFn.FastSetActive(weakPoint, false);
            UnityFn.SetAllInactivate(new List<GameObject>(modSpawnPoints));
            UnityFn.SetAllInactivate(new List<GameObject>(laserShooters));
        }

        void Update()
        {
            if (phase == 0) TriggerIfPlayerIsInRange(storeData, GetDetectionRange(), HandlePhase0ActivateBoss);
            if (phase == 1) HandlePhase1();
            if (phase == 2) HandlePhase2();
            if (phase == 3) HandlePhase3();
            if (phase == 4) HandlePhase4();
        }

        private void HandlePhase0ActivateBoss(Transform closestPlayer)
        {
            musicController.PlayLv8BossMusic1();
            bossCamera.SetActive(true);
            gameCamera.SetActive(false);
            phase = 1;
        }

        private void HandlePhase1()
        {
            bool isMoving = MovementUtil.MoveToPositionOverTime(bossBody.transform, bossBodyHeadTargetPosition, 0.1f, 3);
            if (!isMoving) phase = 2;
        }

        private void HandlePhase2()
        {
            UnityFn.FastSetActive(weakPoint, true);
            UnityFn.SetAllActivate(new List<GameObject>(modSpawnPoints));
            UnityFn.SetAllActivate(new List<GameObject>(laserShooters));
            phase = 3;
        }

        private void HandlePhase3()
        {
            if (weakPointController.isBroken) phase = 4;
        }

        private void HandlePhase4()
        {
            phase = 5; // this is just to prevent getting into here again

            Deactivate();
            AppMusic.instance.Stop();
            UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 5f);
            AbstractDestructibleController.KillAllByType<EnemyWalkerController>();
            AbstractDestructibleController.KillAllByType<Enemy3DFollowerController>();
            AbstractDestructibleController.KillAllByType<EnemyBubbleController>();
            UnityFn.SetTimeout(AppResource.instance, 5, () =>
            {
                AppSfx.instance.levelClear.Play();
                UnityFn.SetTimeout(AppResource.instance, 5, UnityFn.LoadNextScene);
                Destroy(gameObject);
            });
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}