using System;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemySpawnPointController : AbstractRangeDetectionController
    {
        [SerializeField] private int spawnInterval = 5;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float detectionRange = 60f;
        [SerializeField] private int numberPerSpawn = 3;
        [SerializeField] private float repeatDelay = 0.5f;
        [SerializeField] private Vector3 spawnPositionDelta = Vector3.zero;
        [SerializeField] private GameObject animationObject;

        private GameStoreData storeData;
        private IntervalState spawnIntervalState;
        private bool isBroken;
        private Animator animatorCtrl;
        private static readonly int isSpawningKey = Animator.StringToHash("isSpawning");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            spawnIntervalState = IntervalState.Create(spawnInterval);
            if (!enemyPrefab) enemyPrefab = AppResource.instance.enemyWalkerPrefab;
            if (animationObject) animatorCtrl = animationObject.GetComponent<Animator>();
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                UnityFn.RepeatWithInterval(AppResource.instance, spawnIntervalState, numberPerSpawn, repeatDelay, () =>
                {
                    if (!closestPlayer) return;
                    SpawnMod();
                });
            });
        }

        void SpawnMod()
        {
            if (!isBroken)
            {
                if (animatorCtrl) animatorCtrl.SetTrigger(isSpawningKey);
                Instantiate(enemyPrefab, transform.position + spawnPositionDelta, Quaternion.identity);
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }

        private void OnDestroy()
        {
            isBroken = true;
        }
    }
}