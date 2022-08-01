using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace BaseUtil.GameUtil.Util3D
{
    public static class PlayerActionHandler3D
    {
        public static void MoveXZ(UserInput userInput, Rigidbody rb, float moveSpeed)
        {
            Vector3 currentV = rb.velocity;
            rb.velocity = new Vector3(userInput.fixedHorizontal * moveSpeed, currentV.y, userInput.fixedVertical * moveSpeed);
        }

        /// <summary>
        /// Ways to handle movement (test after building game):
        /// * rb.velocity - applies speed, offers good control, need to use FixedUpdate() and set rb to RigidbodyInterpolation.Interpolate to avoid jittery 
        /// * rb.AddForce() - applies acceleration, which is bad for position based control
        /// * transform.Translate() - used in Update() with Time.deltaTime, it creates smooth movement, but it has problems on collision
        /// </summary>
        public static void MoveX(float inputHorizontal, Rigidbody rb, float moveSpeed)
        {
            Vector3 currentV = rb.velocity;
            rb.velocity = new Vector3(inputHorizontal * moveSpeed, currentV.y, currentV.z);
        }

        public static void IcyMoveX(float inputHorizontal, Rigidbody rb, float moveSpeed)
        {
            Vector3 currentV = rb.velocity;
            Vector3 desiredV = new(inputHorizontal * moveSpeed, currentV.y, currentV.z);
            Vector3 smoothVelocity = Vector3.zero;
            rb.velocity = Vector3.SmoothDamp(currentV, desiredV, ref smoothVelocity, 0.04f);
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
            if (isOnGround) UnityFn.HandleJump(rb, jumpForce);
        }

        /**
         * @return new isInDoubleJumpStatus
         */
        private static bool HandleJumpFromGroundOrDoubleJump(bool isOnGround, bool isInDoubleJumpStatus, Rigidbody rb, float jumpForce)
        {
            return GameLogic.HandleJumpFromGroundOrDoubleJump(isOnGround, isInDoubleJumpStatus, () => UnityFn.HandleJump(rb, jumpForce));
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
    }
}