using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BaseUtil.Base;

namespace BaseUtil.GameUtil
{
    public class PlayerActionHandler2D
    {
        public static void HandleHorizontalMovement(float inputHorizontal, Rigidbody2D rb, float moveSpeed)
        {
            rb.velocity = new Vector2(inputHorizontal * moveSpeed, rb.velocity.y);
        }

        /**
         * @return new isInDoubleJumpStatus
         */
        public static bool HandleJump(bool isOnGround, bool isInDoubleJumpStatus, Rigidbody2D rb, float jumpForce, bool hasDoubleJumpAbility)
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

        public static void HandleJumpFromGround(bool isOnGround, Rigidbody2D rb, float jumpForce)
        {
            if (isOnGround) HandleJump(rb, jumpForce);
        }

        /// <summary>
        /// Assists implementation of low jump, so that jumping high needs to press and hold the jump button
        /// </summary>
        /// <param name="rb"></param>
        /// <param name="isOnGround"></param>
        public static void HandleJumpCancelling(Rigidbody2D rb, bool isOnGround)
        {
            if (isOnGround) return; // if on ground, don't handle
            Vector2 velocity = rb.velocity;
            if (velocity.y <= 0f) return; // if falling down, don't handle jump cancelling 
            rb.velocity = new Vector2(velocity.x, velocity.y * 0.1f);
        }

        /**
         * @return new isInDoubleJumpStatus
         */
        private static bool HandleJumpFromGroundOrDoubleJump(bool isOnGround, bool isInDoubleJumpStatus, Rigidbody2D rb, float jumpForce)
        {
            return GameLogic.HandleJumpFromGroundOrDoubleJump(isOnGround, isInDoubleJumpStatus, () => HandleJump(rb, jumpForce));
        }

        public static void HandleJump(Rigidbody2D rb, float jumpForce)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        public static void HandleDash(Rigidbody2D rb, Transform playerTransform, float dashSpeed)
        {
            // Under dashing status, this needs to execute per frame, so that the speed is maintained (not just one frame), until dashing is finished
            rb.velocity = new Vector2(playerTransform.localScale.x * dashSpeed, rb.velocity.y);
        }

        public static float HandleRollingAndStandingToggleWithDirection(bool inputDown, bool inputUp, float switchingCounter, float deltaTime,
                                                                        GameObject standingSprite, GameObject rollingSprite, bool hasSmallShapeAbility)
        {
            if (!hasSmallShapeAbility) return switchingCounter;
            float delayToSwitch = 0.5f; // this is to make sure user doesn't accidentially hit Down() and switch too quick

            if (!rollingSprite.activeSelf)
            {
                if (inputDown)
                {
                    switchingCounter -= deltaTime;
                    if (switchingCounter <= 0)
                    {
                        standingSprite.SetActive(false);
                        rollingSprite.SetActive(true);
                    }
                }
                else
                {
                    switchingCounter = delayToSwitch;
                }
            }
            else
            {
                if (inputUp)
                {
                    switchingCounter -= deltaTime;
                    if (switchingCounter <= 0)
                    {
                        standingSprite.SetActive(true);
                        rollingSprite.SetActive(false);
                    }
                }
                else
                {
                    switchingCounter = delayToSwitch;
                }
            }
            return switchingCounter;
        }

        public static void ToggleRollingAndStanding(bool toggleButton, GameObject standingSprite, GameObject rollingSprite, bool hasSmallShapeAbility)
        {
            if (!hasSmallShapeAbility) return;
            if (toggleButton)
            {
                if (!rollingSprite.activeSelf)
                {
                    standingSprite.SetActive(false);
                    rollingSprite.SetActive(true);
                }
                else
                {
                    standingSprite.SetActive(true);
                    rollingSprite.SetActive(false);
                }
            }
        }

        public static void HandleDuckingDown(bool holdingDownButton, GameObject standingSprite, GameObject duckingSprite)
        {
            if (holdingDownButton)
            {
                standingSprite.SetActive(false);
                duckingSprite.SetActive(true);
            }
            else
            {
                standingSprite.SetActive(true);
                duckingSprite.SetActive(false);
            }
        }
    }
}