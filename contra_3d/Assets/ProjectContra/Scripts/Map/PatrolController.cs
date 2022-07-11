using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class PatrolController : AbstractRangeDetectionController
    {
        [SerializeField] private float movementSpeed = 2f;
        [SerializeField] private float waitTimeAtDestination = 1f;
        [SerializeField] private Vector3 positionDelta = new Vector3(0f, 3f, 0f);
        [SerializeField] private GameObject objectWithAnimation;
        [SerializeField] private float detectionRange = 5f;

        private GameStoreData storeData;

        private Vector3 originalPosition;
        private Vector3 targetPosition;
        private Vector3[] positions;
        private int destinationIndex;
        private IntervalState changeDirectionInterval;
        private Animator animatorCtrl;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            targetPosition = transform.position + positionDelta;
            originalPosition = targetPosition - positionDelta; // not = transform.position to avoid using reference
            positions = new[] {originalPosition, targetPosition};
            destinationIndex = 1;
            changeDirectionInterval = IntervalState.Create(waitTimeAtDestination * 1.1f); // * 1.1f so that only change direction once within wait timer period
            if (objectWithAnimation != null) animatorCtrl = objectWithAnimation.GetComponent<Animator>();
            if (animatorCtrl != null) animatorCtrl.enabled = false;
        }

        void Update()
        {
            TriggerIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                if (animatorCtrl != null) animatorCtrl.enabled = true;
            });
            if (isTriggered) Patrol();
        }

        void Patrol()
        {
            Vector3 currentP = transform.position;
            Vector3 targetP = positions[destinationIndex];
            bool isOverlapping = Vector3.Distance(currentP, targetP) <= 0.1f;

            if (isOverlapping)
            {
                UnityFn.RunWithInterval(this, changeDirectionInterval, () =>
                {
                    UnityFn.SetTimeout(this, waitTimeAtDestination, () =>
                    {
                        destinationIndex = (destinationIndex == 0) ? 1 : 0;
                    });
                });
            }
            else
            {
                transform.position = Vector3.MoveTowards(currentP, targetP, movementSpeed * Time.deltaTime);
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}