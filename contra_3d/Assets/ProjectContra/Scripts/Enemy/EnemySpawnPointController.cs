using System;
using BaseUtil.Base;
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

        [SerializeField] private Vector3 throwModForce = Vector3.zero; // if this is zero, it doesn't throw mod after spawning, otherwise it does
        [SerializeField] private Vector3 throwForceDelta = Vector3.zero; // if provided, delta creates a random range for x, y axis; fixed +z value
        [SerializeField] private float clearThrowVelocityTimeout = 1f; // after e.g. 1 second, it clears the velocity so that the mod can move without affected by the throw effect

        private GameStoreData storeData;
        private IntervalState spawnIntervalState;
        public bool isBroken;
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
                GameObject mod = Instantiate(enemyPrefab, transform.position + spawnPositionDelta, Quaternion.identity);
                ThrowMod(mod);
            }
        }

        void ThrowMod(GameObject mod)
        {
            if (!mod) return;
            if (throwModForce == Vector3.zero) return;
            Rigidbody copyRb = mod.GetComponent<Rigidbody>();
            AbstractDestructibleController modCtrl = mod.GetComponent<AbstractDestructibleController>();
            float x = FnVal.RandomFloatBetween((throwModForce.x - throwForceDelta.x), (throwModForce.x + throwForceDelta.x));
            float y = FnVal.RandomFloatBetween((throwModForce.y - throwForceDelta.y), (throwModForce.y + throwForceDelta.y));
            float z = throwModForce.z + throwForceDelta.z;
            UnityFn.Throw(copyRb, x, y, z);
            UnityFn.SetTimeout(this, clearThrowVelocityTimeout, () =>
            {
                if (!modCtrl.isBroken) copyRb.velocity = Vector3.zero;
            });
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