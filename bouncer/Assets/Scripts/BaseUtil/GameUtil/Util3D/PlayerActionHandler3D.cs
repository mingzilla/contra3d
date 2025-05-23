﻿using System;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace BaseUtil.GameUtil.Util3D
{
    public static class PlayerActionHandler3D
    {
        [Obsolete("Deprecated, use rigidbody instead")]
        public static void MoveXZ(Transform transform, UserInput userInput, float speed, float deltaTime)
        {
            float x = speed * deltaTime * userInput.horizontal;
            float z = speed * deltaTime * userInput.vertical;
            transform.Translate(x, 0, z);
        }

        public static void MoveX(float inputHorizontal, Rigidbody rb, float moveSpeed)
        {
            Vector3 currentV = rb.velocity;
            rb.velocity = new Vector3(inputHorizontal * moveSpeed, currentV.y, currentV.z);
        }

        public static void Rotate(Rigidbody rb, Transform transform, Vector3 rotation)
        {
            rb.freezeRotation = true; // without freezing rotation, collision conflicts with our rotation control
            transform.Rotate(rotation);
            rb.freezeRotation = false;
        }

        /**
         * @return new isInDoubleJumpStatus
         */
        public static bool HandleJump(bool isOnGround, bool isInDoubleJumpStatus, Rigidbody rb, float jumpForce, bool hasDoubleJumpAbility)
        {
            if (hasDoubleJumpAbility)
            {
                return HandleJumpFromGroundOrDoubleJump(isOnGround, isInDoubleJumpStatus, rb, jumpForce);
            }
            else
            {
                HandleJumpFromGround(isOnGround, rb, jumpForce);
                return isInDoubleJumpStatus;
            }
        }

        public static void HandleJumpFromGround(bool isOnGround, Rigidbody rb, float jumpForce)
        {
            if (isOnGround) GameFn.HandleJump(rb, jumpForce);
        }

        /**
         * @return new isInDoubleJumpStatus
         */
        private static bool HandleJumpFromGroundOrDoubleJump(bool isOnGround, bool isInDoubleJumpStatus, Rigidbody rb, float jumpForce)
        {
            return GameLogic.HandleJumpFromGroundOrDoubleJump(isOnGround, isInDoubleJumpStatus, () => GameFn.HandleJump(rb, jumpForce));
        }

        /// <summary>
        /// Assists implementation of low jump, so that jumping high needs to press and hold the jump button
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="isOnGround"></param>
        public static void HandleJumpCancelling(Rigidbody rb, bool isOnGround)
        {
            if (isOnGround) return; // if on ground, don't handle
            Vector3 velocity = rb.velocity;
            if (velocity.y <= 0f) return; // if falling down, don't handle jump cancelling 
            rb.velocity = new Vector3(velocity.x, velocity.y * 0.1f, velocity.z);
        }

        /// <summary>
        /// Combined with jumpForce applied to jumping, to avoid characters being floaty
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="gravityMultiplier"></param>
        public static void HandleGravityModification(Rigidbody rb, float gravityMultiplier)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        public static void HandleLeftRightFacing(Transform playerTransform, bool isFacingRight)
        {
            // https://stackoverflow.com/questions/4023161/whats-a-quaternion-rotation
            float y = isFacingRight ? 1f : -1f;
            playerTransform.rotation = Quaternion.AngleAxis(90f, new Vector3(0f, y, 0f)); // apply 90 degrees to y, 1 means 90d, 0 means 0d, -1 means -90d
        }
    }
}