using UnityEngine;

namespace BaseUtil.GameUtil
{
    public static class PlayerActionHandler3D
    {
        public static void MoveXZ(Transform transform, UserInput userInput, float speed, float deltaTime)
        {
            float x = speed * deltaTime * userInput.horizontal;
            float z = speed * deltaTime * userInput.vertical;
            transform.Translate(x, 0, z);
        }

        public static void Rotate(Rigidbody rb, Transform transform, Vector3 rotation)
        {
            rb.freezeRotation = true; // without freezing rotation, collision conflicts with our rotation control
            transform.Rotate(rotation);
            rb.freezeRotation = false;
        }
    }
}