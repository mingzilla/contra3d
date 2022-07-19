using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv7Controller : AbstractRangeDetectionController
    {
        private GameStoreData storeData;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;

        [SerializeField] private int detectionRange = 50;
        [SerializeField] private GameObject weakPoint;
        [SerializeField] private GameObject[] modSpawnPoints;
        [SerializeField] private GameObject bigMouth;
        [SerializeField] private GameObject body;

        private EnemyBossWeakPointController weakPointController;
        private AppMusic musicController;
        private int phase = 0;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            weakPointController = weakPoint.GetComponent<EnemyBossWeakPointController>();
            Deactivate();
        }

        private void Deactivate()
        {
            UnityFn.FastSetActive(weakPoint, false);
            UnityFn.FastSetActive(bigMouth, false);
            UnityFn.FastSetActive(body, false);
            UnityFn.SetAllInactivate(new List<GameObject>(modSpawnPoints));
        }

        void Update()
        {
            if (phase == 0) TriggerIfPlayerIsInRange(storeData, GetDetectionRange(), HandlePhase0ActivateBoss);
            if (phase == 1) HandlePhase1();
            if (phase == 2) HandlePhase2();
        }

        private void HandlePhase0ActivateBoss(Transform closestPlayer)
        {
            UnityFn.SetTimeout(this, 2f, () =>
            {
                musicController.PlayLv7BossMusic1();
                bossCamera.SetActive(true);
                gameCamera.SetActive(false);
                UnityFn.FastSetActive(weakPoint, true);
                UnityFn.FastSetActive(bigMouth, true);
                UnityFn.FastSetActive(body, true);
                UnityFn.SetAllActivate(new List<GameObject>(modSpawnPoints));
                phase = 1;
            });
        }

        private void HandlePhase1()
        {
            if (weakPointController.isBroken) phase = 2;
        }

        private void HandlePhase2()
        {
            phase = 3; // this is just to prevent getting into here again
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