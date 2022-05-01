using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyWalkerSpawnPointController : EnemyController
    {
        public float spawnInterval = 5f;
        public float detectionRange = 60f;

        private GameStoreData storeData;
        private bool canSpawn = true;

        void Start()
        {
            storeData = AppResource.instance.storeData;
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                UnityFn.RunWithInterval(this, spawnInterval, canSpawn, (s) => canSpawn = s, () =>
                {
                    SpawnWalker();
                    UnityFn.SetTimeout(this, 0.5f, SpawnWalker);
                    UnityFn.SetTimeout(this, 1f, SpawnWalker);
                });
            });
        }

        void SpawnWalker()
        {
            Instantiate(AppResource.instance.enemyWalkerPrefab, transform.position, Quaternion.identity);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
        }
    }
}