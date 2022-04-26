using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton
{
    public class CameraTargetController : MonoBehaviour
    {
        private GameStoreData storeData;

        void Start()
        {
            storeData = AppResource.instance.storeData;
        }

        void FixedUpdate()
        {
            CameraHandler3D.FollowPlayers(transform, storeData.AllPlayerPositions());
        }
    }
}