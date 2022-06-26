using BaseUtil.GameUtil;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class LiftController : MonoBehaviour
    {
        [SerializeField] private GameObject liftTrigger;
        [SerializeField] private GameObject liftDoor;
        [SerializeField] private int liftDoorMoveAmount = -7;
        [SerializeField] private float liftMovementDistanceY = 250f;
        [SerializeField] private float moveSpeed = 1f;

        private Vector3 liftDestination;
        private TriggerByAllPlayersEnterController liftTriggerCtrl;
        private bool isDoorClosed = false;
        private bool isArrived = false;

        void Start()
        {
            liftDestination = transform.position + new Vector3(0f, liftMovementDistanceY, 0f);
            liftTriggerCtrl = liftTrigger.GetComponent<TriggerByAllPlayersEnterController>();
        }

        private void Update()
        {
            if (!liftTriggerCtrl.isActivated) return;
            if (!isDoorClosed)
            {
                liftDoor.transform.position += new Vector3(liftDoorMoveAmount, 0f, 0f);
                isDoorClosed = true;
            }
            if (!isArrived)
            {
                float vDistance = Mathf.Abs(transform.position.y - liftDestination.y);
                isArrived = vDistance < 1;
            }
            if (isArrived) return;
            MovementUtil.MoveY(transform, 1, moveSpeed);
        }
    }
}