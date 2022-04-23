using System.Collections;
using System.Collections.Generic;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class HorizontalMapController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.layer = GameLayer.GROUND.GetLayer();
        }
    }
}