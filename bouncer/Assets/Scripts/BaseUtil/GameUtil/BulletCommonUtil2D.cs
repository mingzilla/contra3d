using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.GameUtil.Domain;
using BaseUtil.GameUtil.Base;
using System;

namespace BaseUtil.GameUtil
{
    public class BulletCommonUtil2D
    {
        public static void HandleBulletMovement(Rigidbody2D theRB, Vector2 moveDirection, float bulletSpeed)
        {
            theRB.velocity = moveDirection * bulletSpeed;
        }

        public static Vector2 CreateBulletDirection(float playerTransformLocalScaleX, UserInput userInput, bool isOnGround)
        {
            // Y: 0f - 0D, 1f - 45D, 2f - 90D
            Vector2 direction = new Vector2(playerTransformLocalScaleX, 0f);

            if (userInput.IsStraightUp()) direction = new Vector2(0, 1f);
            if (userInput.IsStraightDown() && !isOnGround) direction = new Vector2(0, -1f); // if not jumping, the shooting direction won't change
            if (userInput.IsStraightLeft()) direction = new Vector2(playerTransformLocalScaleX, 0f);
            if (userInput.IsStraightRight()) direction = new Vector2(playerTransformLocalScaleX, 0f);

            if (userInput.IsUpLeft()) direction = new Vector2(playerTransformLocalScaleX, 1f);
            if (userInput.IsUpRight()) direction = new Vector2(playerTransformLocalScaleX, 1f);
            if (userInput.IsDownLeft()) direction = new Vector2(playerTransformLocalScaleX, -1f);
            if (userInput.IsDownRight()) direction = new Vector2(playerTransformLocalScaleX, -1f);

            return direction;
        }
    }
}