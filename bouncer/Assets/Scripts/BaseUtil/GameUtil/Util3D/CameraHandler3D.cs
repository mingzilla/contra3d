using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace BaseUtil.GameUtil.Util3D
{
    public static class CameraHandler3D
    {
        public static void FollowPlayers(Transform cameraTarget, List<Vector3> playerPositions)
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
            Vector3 cameraPosition = cameraTarget.position;
            float playersMaxY = MaxYDistanceFromPlayerToCameraCenter(camera.aspect, cameraPosition, playerPositions);
            float z = CalculateCameraZ(playersMaxY);
            cameraTarget.position = new Vector3(position.x, position.y, -(z * camera.aspect));
        }

        public static float CalculateCameraZ(float playersMaxY)
        {
            float minY = CameraUtil.GetMainCameraHalfHeight();
            float maxY = minY * 4f;
            float adjustment = minY / 2f; // distance to the screen border
            float overflowDistance = playersMaxY + adjustment - minY;
            return Mathf.Clamp(overflowDistance, 0, maxY);
        }

        public static float MaxYDistanceFromPlayerToCameraCenter(float cameraAspect, Vector3 cameraPosition, List<Vector3> playerPositions)
        {
            if (playerPositions.Count == 0) return 0;
            List<float> distancesY = Fn.Map(p => Mathf.Abs(p.y - cameraPosition.y), playerPositions);
            List<float> distancesX = Fn.Map(p => Mathf.Abs(p.x - cameraPosition.x), playerPositions);
            List<float> distancesXAsY = Fn.Map(x => (x / cameraAspect), distancesX);
            List<float> everyDistanceAsY = Fn.ConcatAll(new List<List<float>> {distancesY, distancesXAsY});

            return Mathf.Max(everyDistanceAsY.ToArray());
        }
    }
}