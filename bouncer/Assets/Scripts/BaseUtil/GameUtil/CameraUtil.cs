using UnityEngine;

namespace BaseUtil.GameUtil
{
    public static class CameraUtil
    {
        public static float GetMainCameraHalfHeight()
        {
            // Camera.main used to be slow, but now it's not and is actually recommended
            return Camera.main.orthographicSize;
        }

        public static float GetMainCameraHalfWidth()
        {
            Camera camera = Camera.main;
            return camera.orthographicSize * camera.aspect;
        }
    }
}