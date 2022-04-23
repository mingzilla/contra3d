using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class HorizontalMapController : MonoBehaviour
    {
        private MeshCollider meshCollider;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.layer = GameLayer.GROUND.GetLayer();
            meshCollider = UnityFn.AddNoFrictionMeshCollider(gameObject);
        }
    }
}