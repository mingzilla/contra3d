using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUtil.GameUtil
{
    public class DestroyOverTime : MonoBehaviour
    {
        public float lifeTime;

        void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}