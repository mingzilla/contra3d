using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BaseUtil.GameUtil
{
    public class StatusCheck
    {
        public static bool IsOnGround(Transform groundPoint, LayerMask groundLayers)
        {
            // check: within circle close to groundPoint, is there any ground
            // .2f is a good value 
            return Physics2D.OverlapCircle(groundPoint.position, .2f, groundLayers);
        }
    }
}