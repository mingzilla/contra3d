using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D.CameraDomain;
using UnityEngine;

namespace BaseUtil.GameUtil.Util3D
{
    public static class CameraHandler3D
    {
        public static void FollowPlayers(Transform cameraTarget, List<Vector3> playerPositions, bool moveXZ)
        {
            Camera camera = Camera.main;
            if (camera == null) return;
            if (playerPositions.Count == 0) return;
            if (playerPositions.Count == 1)
            {
                cameraTarget.position = playerPositions[0];
                return;
            }

            Vector3 position = UnityFn.GetMeanVector3(playerPositions);
            Vector3 delta;
            if (moveXZ)
            {
                delta = CameraTargetParamXZ.CalculateVector3Delta(new CameraTargetParamXZ(), playerPositions);
            }
            else
            {
                delta = CameraTargetParamX.CalculateVector3Delta(new CameraTargetParamX(), playerPositions);
            }
            cameraTarget.position = position + delta;
        }
    }
}