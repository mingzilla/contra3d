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

        /// <summary>
        /// Used in Update Loop to allow unit constantly following target. The unit immediately rotates to the target.
        /// </summary>
        /// <returns>If unit is already overlapping the target within the given range</returns>
        public static bool FollowTowardsPosition3D(Transform unitTransform, Transform targetTransform,
                                                   float overlapRange, float moveSpeed, float deltaTime)
        {
            bool isOverlapping = UnityFn.IsInRange(unitTransform, targetTransform, overlapRange);
            if (isOverlapping) return true;

            unitTransform.rotation = UnityFn.GetImmediateRotation3D(unitTransform.position, targetTransform.position);
            unitTransform.position = UnityFn.GetPosition(unitTransform, targetTransform, moveSpeed, deltaTime);
            return false;
        }

        public static bool FollowXZTowardsPosition3D(Transform unitTransform, Transform targetTransform,
                                                     float overlapRange, float moveSpeed, float deltaTime)
        {
            bool isOverlapping = UnityFn.IsInRange(unitTransform, targetTransform, overlapRange);
            if (isOverlapping) return true;

            unitTransform.rotation = UnityFn.LookXZ(unitTransform, targetTransform);
            unitTransform.position = UnityFn.GetPositionXZ(unitTransform, targetTransform, moveSpeed, deltaTime);
            return false;
        }

        /// <summary>
        /// Used in Update Loop Constantly move towards one direction by applying addDeltaToTargetFn. e.g. bullets
        /// If target position doesn't need to keep changing, use MoveToPositionOverTime()
        /// </summary>
        public static void MoveTowardsPosition3D(Transform transform, Vector3 targetPosition, float moveSpeed, Action<Vector3> addDeltaToTargetFn)
        {
            Vector3 originalPosition = transform.position;
            Vector3 delta = UnityFn.GetFramePositionDelta(originalPosition, targetPosition, moveSpeed, Time.deltaTime);
            transform.position = originalPosition + delta; // same as Vector3.MoveTowards(), using delta is just to avoid calculating it again
            addDeltaToTargetFn(delta); // targetPosition += delta; - move target further so that bullet never catches the target
        }

        public static void MoveToPositionOverTime(Transform transform, Vector3 targetPosition, float overlapRange, float moveSpeed)
        {
            Vector3 position = transform.position;
            if (Vector3.Distance(position, targetPosition) <= overlapRange) return;
            transform.position = Vector3.MoveTowards(position, targetPosition, moveSpeed * Time.deltaTime);
        }

        /// <param name="xValue">-1 goes left, 1 goes right</param>
        public static void MoveX(Transform transform, int xValue, float moveSpeed)
        {
            float x = (moveSpeed * Time.deltaTime) * ((xValue > 0) ? 1 : -1);
            transform.position += new Vector3(x, 0f, 0f);
        }

        /// <param name="yValue">-1 goes down, 1 goes up</param>
        public static void MoveY(Transform transform, int yValue, float moveSpeed)
        {
            float y = (moveSpeed * Time.deltaTime) * ((yValue > 0) ? 1 : -1);
            transform.position += new Vector3(0f, y, 0f);
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
        public static Vector3 MoveLikePlatform(Vector3 startingPosition, Vector3 furthestRelativePosition, float period)
        {
            if (period <= Mathf.Epsilon) return startingPosition;
            float rawSinWave = CalculateCircularMovementFactor(period);
            float movementFactor = (rawSinWave + 1f) / 2f; // between 0 to 1
            Vector3 offset = furthestRelativePosition * movementFactor;
            return startingPosition + offset;
        }

        /// <summary>
        /// Value between -1 to 1 over time
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public static float CalculateCircularMovementFactor(float period)
        {
            float cycles = Time.time / period;
            const float tau = Mathf.PI * 2f;
            float rawSinWave = Mathf.Sin(cycles * tau); // between -1 to 1
            return rawSinWave;
        }
    }
}