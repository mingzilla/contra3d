using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
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
            SceneInitData sceneInitData = AppResource.instance.GetCurrentSceneInitData();
            CameraHandler3D.FollowPlayers(transform, storeData.AllPlayerPositions(), sceneInitData.moveXZ);
        }
    }
}