using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace BaseUtil.GameUtil.Util3D.CameraDomain
{
    public class CameraTargetParamX
    {
        public float defaultPlayersDistance = 4f; // If players distance is lower than this, the camera scale doesn't change. Put 2 players on the screen, before one is out of screen, their distance is the default distance
        public float maxPlayersDistance = 80f; // If player distance is bigger than this, use camera max scale
        public float maxDeltaZ = 35f; // Camera scale changes based on setting deltaZ, this controls the max scale
        public float deltaYRatio = 0.2f; // deltaY == deltaZ * deltaYRatio

        public static Vector3 CalculateVector3Delta(CameraTargetParamX cameraTargetParamX, List<Vector3> playerPositions)
        {
            Vector3 maxPlayersDistance = UnityFn.GetMaxVector3Distance(playerPositions);
            float currentPlayersDistance = Vector3.Distance(maxPlayersDistance, Vector3.zero);
            float deltaZ = CalculateDeltaZ(cameraTargetParamX, currentPlayersDistance);
            float deltaY = deltaZ * cameraTargetParamX.deltaYRatio;
            return new Vector3(0, deltaY, -deltaZ);
        }

        /// <summary>
        /// Used by X movement, which adjusts -Z of the camera target to decide the scale of the camera.
        /// </summary>
        public static float CalculateDeltaZ(CameraTargetParamX cameraTargetParamX, float currentPlayersDistance)
        {
            float defaultPlayersDistance = cameraTargetParamX.defaultPlayersDistance;
            float maxPlayersDistance = cameraTargetParamX.maxPlayersDistance;
            float maxDeltaZ = cameraTargetParamX.maxDeltaZ;

            if (currentPlayersDistance <= defaultPlayersDistance) return 0f;
            if (currentPlayersDistance >= maxPlayersDistance) return maxDeltaZ;
            return (currentPlayersDistance - defaultPlayersDistance) / (maxPlayersDistance - defaultPlayersDistance) * maxDeltaZ;
        }
    }
}