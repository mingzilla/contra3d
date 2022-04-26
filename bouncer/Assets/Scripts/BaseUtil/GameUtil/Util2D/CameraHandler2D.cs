using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using UnityEngine;

namespace BaseUtil.GameUtil.Util2D
{
    public static class CameraHandler2D
    {
        public static void FollowPlayers(Transform cameraTransform, BoxCollider2D cameraBound, List<Vector3> playerPositions)
        {
            if (playerPositions.Count == 0) return;
            Vector3 position = UnityFn.GetMeanVector3(playerPositions);
            if (cameraBound == null) FollowPlayer(cameraTransform, position.x, position.y);
            if (cameraBound != null) FollowPlayerWithBoundary(cameraTransform, cameraBound, position);
        }

        public static void FollowPlayerWithBoundary(Transform cameraTransform, BoxCollider2D cameraBound, Vector3 playerPosition)
        {
            float halfHeight = CameraUtil.GetMainCameraHalfHeight();
            float halfWidth = CameraUtil.GetMainCameraHalfWidth();

            // apply bounds, and use half sizes to make sure it doesn't move out
            Bounds bounds = cameraBound.bounds;
            float x = Mathf.Clamp(playerPosition.x, bounds.min.x + halfWidth, bounds.max.x - halfWidth);
            float y = Mathf.Clamp(playerPosition.y, bounds.min.y + halfHeight, bounds.max.y - halfHeight);

            FollowPlayer(cameraTransform, x, y);
        }

        public static void FollowPlayer(Transform cameraTransform, float x, float y)
        {
            // x, y follow player, z stays the same, see nothing if player and camera have the same z position
            Vector3 originalPosition = cameraTransform.position;
            Vector3 targetPosition = new Vector3(x, y, originalPosition.z);
            Vector3 currentVelocity = Vector3.zero; // e.g. if set to Vector3.one, the camera keeps moving towards the screen 
            float smoothTime = 0.05f; // e.g. if set to 0.3f, it moves slow like spaceship games

            // https://docs.unity3d.com/ScriptReference/Vector3.SmoothDamp.html
            cameraTransform.position = Vector3.SmoothDamp(originalPosition, targetPosition, ref currentVelocity, smoothTime);
        }

        public static void HandleCameraSize2D(Vector3 cameraPosition, List<Vector3> playerPositions)
        {
            HandleCameraSize(15f, 25f, cameraPosition, playerPositions);
        }

        public static void HandleCameraSize(float minY, float maxY, Vector3 cameraPosition, List<Vector3> playerPositions)
        {
            Camera camera = Camera.main;
            if (camera == null) return;
            camera.orthographicSize = CalculateCameraSize(minY, maxY, camera.aspect, cameraPosition, playerPositions);
        }

        /**
         * Calculates Camera orthographicSize, 
         */
        public static float CalculateCameraSize(float minY, float maxY, float cameraAspect, Vector3 cameraPosition, List<Vector3> playerPositions)
        {
            if (playerPositions.Count == 0) return minY;
            List<float> distancesY = Fn.Map(p => Mathf.Abs(p.y - cameraPosition.y), playerPositions);
            List<float> distancesX = Fn.Map(p => Mathf.Abs(p.x - cameraPosition.x), playerPositions);
            List<float> distancesXAsY = Fn.Map(x => (x / cameraAspect), distancesX);
            List<float> everyDistanceAsY = Fn.ConcatAll(new List<List<float>> {distancesY, distancesXAsY});

            float maxDistanceY = Mathf.Max(everyDistanceAsY.ToArray());

            // (At the moment this conflicts with camera bounds, camera bounds create such adjustment)
            float adjustment = 6f; // distance to the screen border

            float result = Mathf.Clamp(maxDistanceY + adjustment, minY, maxY);
            return result;
        }
    }
}