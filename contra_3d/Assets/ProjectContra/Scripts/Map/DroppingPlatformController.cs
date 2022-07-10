using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class DroppingPlatformController : AbstractRangeDetectionController
    {
        public float detectionRange = 17.5f;
        public float movementSpeed = 2f;
        public Vector3 positionDelta = new Vector3(0f, -2f, 0f);

        private MeshCollider meshCollider;
        private GameStoreData storeData;

        private Vector3 targetPosition;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.GROUND.GetLayer();
            meshCollider = UnityFn.AddNoFrictionCollider<MeshCollider>(gameObject);
            targetPosition = transform.position + positionDelta;
        }

        void Update()
        {
            TriggerIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                AppSfx.instance.floorMove.Play();
            });
            if (isTriggered) MovementUtil.MoveToPositionOverTime(transform, targetPosition, 0.1f, movementSpeed);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}