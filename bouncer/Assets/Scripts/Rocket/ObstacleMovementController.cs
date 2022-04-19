using System;
using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil;
using Types;
using UnityEngine;

namespace Rocket
{
    public class ObstacleMovementController : MonoBehaviour
    {
        public Vector3 startingPosition;
        public Vector3 furthestRelativePosition = new Vector3(10f, 0, 0);
        public float period = 2f;

        private void Start()
        {
            startingPosition = transform.position;
        }

        private void Update()
        {
            transform.position = MovementUtil.MovePlatform(startingPosition, furthestRelativePosition, period);
        }
    }
}