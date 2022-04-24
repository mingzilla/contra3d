using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BaseUtil.GameUtil.Domain;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Types;

namespace BaseUtil.GameUtil.Util2D
{
    public class UnitDisplayHandler2D
    {
        public static void HandleSpriteFacing(Rigidbody2D theRb, Transform theTransform)
        {
            var childSprites = UnityFn.FindChildrenWithTag(theTransform, GameTag.SPRITE.name);
            foreach (var t in childSprites)
            {
                UnitDisplayHandler2D.HandleFacing(theRb, t);
            }
        }

        public static void HandleFacing(Rigidbody2D theRb, Transform theTransform)
        {
            var velocity = theRb.velocity;
            bool isFacingLeft = velocity.x < 0f;
            bool isFacingRight = velocity.x > 0f;

            // this only works if the standing sprit and ground point have correct relative position to the game object
            // so make sure initially the positions are set to 0 before scraling/moving the relative positions
            var localScale = theTransform.localScale;
            if (isFacingRight)
            {
                theTransform.localScale = new Vector3(1f, localScale.y, localScale.z);
            }
            else if (isFacingLeft)
            {
                theTransform.localScale = new Vector3(-1f, localScale.y, localScale.z);
            }
        }

        public static float HandleDashingImage(SpriteRenderer dashingImage, SpriteRenderer theSr, Transform playerTransform, Color dashingImageColor,
            float dashingImageCloneCounter, float deltaTime)
        {
            dashingImageCloneCounter -= deltaTime;

            if (dashingImageCloneCounter <= 0)
            {
                UnitDisplayHandler2D.ShowDashingImage(dashingImage, theSr, playerTransform, dashingImageColor);
                dashingImageCloneCounter = DashCounter.dashingImageCloneDelay;
            }

            return dashingImageCloneCounter;
        }

        public static void ShowDashingImage(SpriteRenderer dashingImage, SpriteRenderer theSr, Transform playerTransform, Color dashingImageColor)
        {
            SpriteRenderer image = UnityEngine.Object.Instantiate(dashingImage, playerTransform.position, playerTransform.rotation);
            image.sprite = theSr.sprite;
            image.transform.localScale = playerTransform.localScale;
            image.color = dashingImageColor;
            UnityEngine.Object.Destroy(image.gameObject, DashCounter.dashingImageLifeTime);
        }
    }
}