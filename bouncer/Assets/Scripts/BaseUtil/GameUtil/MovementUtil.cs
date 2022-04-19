using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using System;

namespace BaseUtil.GameUtil
{
    public class MovementUtil
    {
        public static bool MoveXTowardsPosition(Rigidbody2D unitRB, Transform unitTransform, Transform targetTransform,
                                                float overlapRange, float moveSpeed, bool imageFaceLeft)
        {
            bool isOverlapping = Mathf.Abs(targetTransform.position.x - unitTransform.position.x) < overlapRange;
            if (isOverlapping) return true;

            bool unitOnLeft = targetTransform.position.x > unitTransform.position.x;
            bool unitOnRight = targetTransform.position.x < unitTransform.position.x;
            float xScaleValueWhenFacingLeft = imageFaceLeft ? -1f : 1f;

            if (unitOnLeft)
            {
                unitRB.velocity = new Vector2(moveSpeed, unitRB.velocity.y);
                unitTransform.localScale = new Vector3(xScaleValueWhenFacingLeft, unitTransform.localScale.y, unitTransform.localScale.z);
            }

            if (unitOnRight)
            {
                unitRB.velocity = new Vector2(-moveSpeed, unitRB.velocity.y);
                unitTransform.localScale = new Vector3((0 - xScaleValueWhenFacingLeft), unitTransform.localScale.y, unitTransform.localScale.z);
            }
            return false;
        }

        public static bool MoveTowardsPositionNoRotation(Rigidbody2D unitRB, Transform unitTransform, Transform targetTransform,
                                                         float overlapRange, float moveSpeed, bool imageFaceLeft)
        {
            bool isOverlappingX = Mathf.Abs(targetTransform.position.x - unitTransform.position.x) < overlapRange;
            bool isOverlappingY = Mathf.Abs(targetTransform.position.y - unitTransform.position.y) < overlapRange;
            bool isOverlapping = isOverlappingX && isOverlappingY;
            if (isOverlapping) return true;

            bool unitOnLeft = targetTransform.position.x > unitTransform.position.x;
            bool unitOnRight = targetTransform.position.x < unitTransform.position.x;
            float xScaleValueWhenFacingLeft = imageFaceLeft ? -1f : 1f;

            bool unitOnTop = targetTransform.position.y < unitTransform.position.y;
            bool unitOnBottom = targetTransform.position.y > unitTransform.position.y;

            if (unitOnLeft) unitRB.velocity = new Vector2(moveSpeed, unitRB.velocity.y);
            if (unitOnRight) unitRB.velocity = new Vector2(-moveSpeed, unitRB.velocity.y);
            if (unitOnTop) unitRB.velocity = new Vector2(unitRB.velocity.x, -moveSpeed);
            if (unitOnBottom) unitRB.velocity = new Vector2(unitRB.velocity.x, moveSpeed);

            if (unitOnLeft) unitTransform.localScale = new Vector3(xScaleValueWhenFacingLeft, unitTransform.localScale.y, unitTransform.localScale.z);
            if (unitOnRight) unitTransform.localScale = new Vector3((0 - xScaleValueWhenFacingLeft), unitTransform.localScale.y, unitTransform.localScale.z);

            return false;
        }

        public static bool MoveTowardsPosition(Transform unitTransform, Transform targetTransform,
                                               float overlapRange, float moveSpeed, float turnSpeed, float deltaTime)
        {
            bool isOverlapping = UnityFn.IsInRange(unitTransform, targetTransform, overlapRange);
            if (isOverlapping) return true;

            unitTransform.rotation = UnityFn.GetRotation2D(unitTransform, targetTransform, turnSpeed, deltaTime);
            unitTransform.position = UnityFn.GetPosition(unitTransform, targetTransform, moveSpeed, deltaTime);
            return false;
        }

        public static bool JumpWhenOverlapsWithPositions(Rigidbody2D unitRB, Transform unitTransform, List<Transform> jumpPoints,
                                                         float overlapRange, float jumpForce)
        {
            bool isOverlapping = Fn.Any((jp) => (Mathf.Abs(jp.position.x - unitTransform.position.x) < overlapRange), jumpPoints);
            if (!isOverlapping) return false;
            unitRB.velocity = new Vector2(unitRB.velocity.x, jumpForce);
            return true;
        }

        /// <summary>
        /// Used by e.g. a moving platform.
        /// Call this in update loop to make an object moving back and forth all the time
        /// </summary>
        /// <param name="startingPosition"></param>
        /// <param name="furthestRelativePosition">the furthest position to move towards, relative to the starting position</param>
        /// <param name="period">how much time we want a cycle to be. e.g. 2f</param>
        /// <returns>New position to be set to transform.position of an object</returns>
        public static Vector3 MovePlatform(Vector3 startingPosition, Vector3 furthestRelativePosition, float period)
        {
            float cycles = Time.time / period;
            const float tau = Mathf.PI * 2;
            float rawSinWave = Mathf.Sin(cycles * tau); // between -1 to 1
            float movementFactor = (rawSinWave + 1f) / 2f; // between 0 to 1
            Vector3 offset = furthestRelativePosition * movementFactor;
            return startingPosition + offset;
        }
    }
}