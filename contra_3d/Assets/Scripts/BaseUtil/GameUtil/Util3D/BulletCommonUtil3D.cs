using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace BaseUtil.GameUtil.Util3D
{
    public static class BulletCommonUtil3D
    {
        public static Rigidbody AddRigidbodyAndColliderToBullet(GameObject gameObject, bool useGravity, float colliderRadius)
        {
            UnityFn.AddSphereCollider(gameObject, colliderRadius, true);
            return UnityFn.AddRigidbody(gameObject, useGravity, false); // e.g. grenade can come from outside of the axis
        }

        public static void HandleBulletFixedMovement(Rigidbody theRB, Vector3 moveDirection, float bulletSpeed)
        {
            theRB.velocity = moveDirection * bulletSpeed;
        }

        /// <param name="envDeltaV">Vertical delta caused by environment movement, e.g. when lift goes up, bullet moves down, so delta is needed to maintain horizontal movement</param>
        /// <returns></returns>
        public static Vector3 CreateBulletDirection(bool isFacingForward, UserInput userInput, bool isOnGround, float envDeltaV)
        {
            float x = isFacingForward ? 1f : -1f;
            float nonStraightY = 0.8f;

            // Y: 0f - 0D, 1f - 45D, 2f - 90D
            Vector3 direction = new Vector3(x, 0f, 0f);

            if (userInput.IsStraightUp()) direction = new Vector3(0, 1f, 0f);
            if (userInput.IsStraightDown() && !isOnGround) direction = new Vector3(0, -1f, 0f); // if not jumping, the shooting direction won't change
            if (userInput.IsStraightLeft()) direction = new Vector3(x, 0f + envDeltaV, 0f);
            if (userInput.IsStraightRight()) direction = new Vector3(x, 0f + envDeltaV, 0f);

            if (userInput.IsUpLeft()) direction = new Vector3(x, nonStraightY, 0f);
            if (userInput.IsUpRight()) direction = new Vector3(x, nonStraightY, 0f);
            if (userInput.IsDownLeft()) direction = new Vector3(x, -nonStraightY, 0f);
            if (userInput.IsDownRight()) direction = new Vector3(x, -nonStraightY, 0f);

            return direction;
        }

        public static Vector3 CreateBulletXZDirection(float rotationEulerAnglesY)
        {
            float rotationY = (rotationEulerAnglesY + 360f) % 360f;
            if (IsAroundNum(0f, rotationY)) return new Vector3(0f, 0f, 1f);
            if (IsAroundNum(45f, rotationY)) return new Vector3(0.8f, 0f, 0.8f);
            if (IsAroundNum(90f, rotationY)) return new Vector3(1f, 0f, 0f);
            if (IsAroundNum(135f, rotationY)) return new Vector3(0.8f, 0f, -0.8f);
            if (IsAroundNum(180f, rotationY)) return new Vector3(0f, 0f, -1f);
            if (IsAroundNum(225f, rotationY)) return new Vector3(-0.8f, 0f, -0.8f);
            if (IsAroundNum(270f, rotationY)) return new Vector3(-1f, 0f, 0f);
            if (IsAroundNum(315f, rotationY)) return new Vector3(-0.8f, 0f, 0.8f);
            return new Vector3(0f, 0f, 1f);
        }

        private static bool IsAroundNum(float num, float y)
        {
            return FnVal.IsBetweenF((num - 22.5f), (num + 22.5f), true, y);
        }
    }
}