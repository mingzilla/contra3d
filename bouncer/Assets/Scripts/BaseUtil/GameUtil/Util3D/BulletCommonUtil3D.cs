using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.GameUtil.Domain;
using BaseUtil.GameUtil.Base;
using System;

namespace BaseUtil.GameUtil.Util3D
{
    public class BulletCommonUtil3D
    {
        public static Rigidbody AddRigidbodyAndColliderToBullet(GameObject gameObject)
        {
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.radius = 1f;
            collider.isTrigger = true;
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            return rb;
        }

        public static void HandleBulletFixedMovement(Rigidbody theRB, Vector3 moveDirection, float bulletSpeed)
        {
            theRB.velocity = moveDirection * bulletSpeed;
        }

        public static Vector3 CreateBulletDirection(bool isFacingForward, UserInput userInput, bool isOnGround)
        {
            float x = isFacingForward ? 1f : -1f;
            float nonStraightY = 0.8f;

            // Y: 0f - 0D, 1f - 45D, 2f - 90D
            Vector3 direction = new Vector3(x, 0f, 0f);

            if (userInput.IsStraightUp()) direction = new Vector3(0, 1f, 0f);
            if (userInput.IsStraightDown() && !isOnGround) direction = new Vector3(0, -1f, 0f); // if not jumping, the shooting direction won't change
            if (userInput.IsStraightLeft()) direction = new Vector3(x, 0f, 0f);
            if (userInput.IsStraightRight()) direction = new Vector3(x, 0f, 0f);

            if (userInput.IsUpLeft()) direction = new Vector3(x, nonStraightY, 0f);
            if (userInput.IsUpRight()) direction = new Vector3(x, nonStraightY, 0f);
            if (userInput.IsDownLeft()) direction = new Vector3(x, -nonStraightY, 0f);
            if (userInput.IsDownRight()) direction = new Vector3(x, -nonStraightY, 0f);

            return direction;
        }
    }
}