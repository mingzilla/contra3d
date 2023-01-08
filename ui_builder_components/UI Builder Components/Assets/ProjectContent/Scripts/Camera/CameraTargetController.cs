using BaseUtil.GameUtil.Util3D;
using ProjectContent.Scripts.AppLiveResource;
using ProjectContent.Scripts.Data;
using UnityEngine;

namespace ProjectContent.Scripts.Camera
{
    public class CameraTargetController : MonoBehaviour
    {
        private GameStoreData storeData;

        void Start()
        {
            storeData = AppResource.instance.storeData;
        }

        void LateUpdate()
        {
            CameraHandler3D.FollowPlayers(transform, storeData.AllPlayerPositions(), true);
        }
    }
}