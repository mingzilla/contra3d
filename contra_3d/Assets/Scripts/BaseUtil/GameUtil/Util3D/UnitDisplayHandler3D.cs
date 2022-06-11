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

        public static void HandleXZFacing(Transform unitTransform, UserInput userInput)
        {
            if (userInput.IsStraightRight()) unitTransform.rotation = Quaternion.AngleAxis(90f, new Vector3(0f, 1f, 0f));
            if (userInput.IsStraightLeft()) unitTransform.rotation = Quaternion.AngleAxis(90f, new Vector3(0f, -1f, 0f));
            if (userInput.IsStraightUp()) unitTransform.rotation = Quaternion.AngleAxis(0f, new Vector3(0f, 0f, 0f));
            if (userInput.IsStraightDown()) unitTransform.rotation = Quaternion.AngleAxis(180f, new Vector3(0f, 1f, 0f));
            
            if (userInput.IsUpLeft()) unitTransform.rotation = Quaternion.AngleAxis(45f, new Vector3(0f, -1f, 0f));
            if (userInput.IsUpRight()) unitTransform.rotation = Quaternion.AngleAxis(45f, new Vector3(0f, 1f, 0f));
            if (userInput.IsDownLeft()) unitTransform.rotation = Quaternion.AngleAxis(135f, new Vector3(0f, -1f, 0f));
            if (userInput.IsDownRight()) unitTransform.rotation = Quaternion.AngleAxis(135f, new Vector3(0f, -1f, 0f));
        }
    }
}