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
    }
}