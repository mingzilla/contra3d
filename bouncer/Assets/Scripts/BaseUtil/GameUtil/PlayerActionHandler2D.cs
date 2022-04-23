using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BaseUtil.Base;

namespace BaseUtil.GameUtil
{
    public class PlayerActionHandler2D
    {
        public static void HandleHorizontalMovement(float inputHorizontal, Rigidbody2D theRB, float moveSpeed)
        {
            theRB.velocity = new Vector2(inputHorizontal * moveSpeed, theRB.velocity.y);
        }

        public static void HandleJumpFromGround(bool isOnGround, Rigidbody2D theRB, float jumpForce)
        {
            if (isOnGround) PlayerActionHandler2D.HandleJump(theRB, jumpForce);
        }

        /**
         * @return new isInDoubleJumpStatus
         */
        public static bool HandleJump(bool isOnGround, bool isInDoubleJumpStatus, Rigidbody2D theRB, float jumpForce, bool hasDoubleJumpAbility)
        {
            if (hasDoubleJumpAbility)
            {
                return PlayerActionHandler2D.HandleJumpFromGroundOrDoubleJump(isOnGround, isInDoubleJumpStatus, theRB, jumpForce);
            }
            else
            {
                PlayerActionHandler2D.HandleJumpFromGround(isOnGround, theRB, jumpForce);
                return isInDoubleJumpStatus;
            }
        }

        /// <summary>
        /// Assists implementation of low jump, so that jumping high needs to press and hold the jump button
        /// </summary>
        /// <param name="theRb"></param>
        /// <param name="isOnGround"></param>
        public static void HandleJumpCancelling(Rigidbody2D theRb, bool isOnGround)
        {
            if (isOnGround) return; // if on ground, don't handle
            Vector2 velocity = theRb.velocity;
            if (velocity.y <= 0f) return; // if falling down, don't handle jump cancelling 
            theRb.velocity = new Vector2(velocity.x, velocity.y * 0.1f);
        }

        /**
         * @return new isInDoubleJumpStatus
         */
        private static bool HandleJumpFromGroundOrDoubleJump(bool isOnGround, bool isInDoubleJumpStatus, Rigidbody2D theRB, float jumpForce)
        {
            return GameLogic.HandleJumpFromGroundOrDoubleJump(isOnGround, isInDoubleJumpStatus, () => PlayerActionHandler2D.HandleJump(theRB, jumpForce));
        }

        public static void HandleJump(Rigidbody2D theRB, float jumpForce)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
        }

        public static void HandleDash(Rigidbody2D theRB, Transform playerTransform, float dashSpeed)
        {
            // Under dashing status, this needs to execute per frame, so that the speed is maintained (not just one frame), until dashing is finished
            theRB.velocity = new Vector2(playerTransform.localScale.x * dashSpeed, theRB.velocity.y);
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