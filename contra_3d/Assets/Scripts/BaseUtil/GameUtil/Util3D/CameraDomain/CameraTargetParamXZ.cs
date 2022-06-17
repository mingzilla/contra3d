using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace BaseUtil.GameUtil.Util3D.CameraDomain
{
    public class CameraTargetParamXZ
    {
        public float defaultPlayersDistance = 20f; // If players distance is lower than this, the camera scale doesn't change. Put 2 players on the screen, before one is out of screen, their distance is the default distance
        public float maxPlayersDistance = 40f; // If player distance is bigger than this, use camera max scale
        public float maxDeltaY = 12f; // Camera scale changes based on setting deltaY, this controls the max scale

        public static Vector3 CalculateVector3Delta(CameraTargetParamXZ cameraTargetParamXZ, List<Vector3> playerPositions)
        {
            Vector3 maxPlayersDistance = UnityFn.GetMaxVector3Distance(playerPositions);
            float currentPlayersDistance = Vector3.Distance(maxPlayersDistance, Vector3.zero);
            float deltaY = CalculateDeltaY(cameraTargetParamXZ, currentPlayersDistance);
            return new Vector3(0, deltaY, 0);
        }

        /// <summary>
        /// Used by XZ movement, which adjusts Y of the camera target to decide the scale of the camera.
        /// </summary>
        public static float CalculateDeltaY(CameraTargetParamXZ cameraTargetParamXZ, float currentPlayersDistance)
        {
            float defaultPlayersDistance = cameraTargetParamXZ.defaultPlayersDistance;
            float maxPlayersDistance = cameraTargetParamXZ.maxPlayersDistance;
            float maxDeltaY = cameraTargetParamXZ.maxDeltaY;

            if (currentPlayersDistance <= defaultPlayersDistance) return 0f;
            if (currentPlayersDistance >= maxPlayersDistance) return maxDeltaY;
            return (currentPlayersDistance - defaultPlayersDistance) / (maxPlayersDistance - defaultPlayersDistance) * maxDeltaY;
        }
    }
}