using System;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyWalkerSpawnPointController : AbstractRangeDetectionController
    {
        [SerializeField] private int spawnInterval = 5;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float detectionRange = 60f;
        [SerializeField] private int numberPerSpawn = 3;
        [SerializeField] private float repeatDelay = 0.5f;

        private GameStoreData storeData;
        private IntervalState spawnIntervalState;
        private bool isBroken;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            spawnIntervalState = IntervalState.Create(spawnInterval);
            if (!enemyPrefab) enemyPrefab = AppResource.instance.enemyWalkerPrefab;
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                UnityFn.RepeatWithInterval(AppResource.instance, spawnIntervalState, numberPerSpawn, repeatDelay, () =>
                {
                    if (!closestPlayer) return;
                    SpawnWalker();
                });
            });
        }

        void SpawnWalker()
        {
            if (!isBroken) Instantiate(enemyPrefab, transform.position, Quaternion.identity);
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