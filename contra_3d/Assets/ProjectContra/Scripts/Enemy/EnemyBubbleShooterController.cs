using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBubbleShooterController : AbstractRangeDetectionController
    {
        public float shotInterval = 0.3f;
        public float detectionRange = 40f;
        [SerializeField] private Vector3 shootPositionDelta = new Vector3(0f, 1f, 0f);
        [SerializeField] private float bubbleInitialVelocityY = -10f;
        [SerializeField] private int bubbleLifeTime = 15;
        [SerializeField] private GameObject bubblePrefab;
        [SerializeField] private int randomRangeX = 5;

        private GameStoreData storeData;
        private IntervalState shotIntervalState;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            shotIntervalState = IntervalState.Create(shotInterval);
            if (!bubblePrefab) bubblePrefab = AppResource.instance.enemyBubblePrefab;
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                UnityFn.RunWithInterval(this, shotIntervalState, () =>
                {
                    Vector3 position = transform.position;
                    float initialX = FnVal.RandomFloatBetween(-(randomRangeX), randomRangeX);
                    Vector3 bubbleInitialVelocity = new(initialX, bubbleInitialVelocityY, 0f);
                    EnemyBubbleController.Spawn(bubblePrefab, position + shootPositionDelta, bubbleInitialVelocity, bubbleLifeTime);
                });
            });
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}