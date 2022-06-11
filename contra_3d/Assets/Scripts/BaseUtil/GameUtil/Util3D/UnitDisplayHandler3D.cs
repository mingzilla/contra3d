using UnityEngine;

namespace BaseUtil.GameUtil.Util3D
{
    public class UnitDisplayHandler3D
    {
        public static void HandleLeftRightFacing(Transform unitTransform, bool isFacingRight)
        {
            // https://stackoverflow.com/questions/4023161/whats-a-quaternion-rotation
            float y = isFacingRight ? 1f : -1f;
            unitTransform.rotation = Quaternion.AngleAxis(90f, new Vector3(0f, y, 0f)); // apply 90 degrees to y, 1 means 90d, 0 means 0d, -1 means -90d
        }
        public static void HandleXZFacing(Transform unitTransform, bool isFacingRight, bool isFacingUp)
        {
            // https://stackoverflow.com/questions/4023161/whats-a-quaternion-rotation
            float y = isFacingRight ? 1f : -1f;
            float z = isFacingUp ? 1f : -1f;
            unitTransform.rotation = Quaternion.AngleAxis(90f, new Vector3(0f, y, z)); // apply 90 degrees to y, 1 means 90d, 0 means 0d, -1 means -90d
        }
    }
}