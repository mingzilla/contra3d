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

        private GameStoreData storeData;
        private IntervalState spawnIntervalState;

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
                UnityFn.RunWithInterval(AppResource.instance, spawnIntervalState, () =>
                {
                    SpawnWalker();
                    UnityFn.SetTimeout(this, 0.5f, SpawnWalker);
                    UnityFn.SetTimeout(this, 1f, SpawnWalker);
                });
            });
        }

        void SpawnWalker()
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}