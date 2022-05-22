using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using BaseUtil.GameUtil.Types;
using BaseUtil.GameUtil.Domain;
using System;

namespace BaseUtil.GameUtil.Base
{
    /**
     * Common things a game needs to do.
     */
    public static class GameFn2D
    {
        public static void DealDamage(Vector3 position, float blastRange, LayerMask affectedLayers, Action<GameObject> damagingFn)
        {
            Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(position, blastRange, affectedLayers);
            foreach (var item in affectedObjects)
            {
                damagingFn(item.gameObject);
            }
        }

        public static bool IsOnGround(Transform groundPoint, LayerMask groundLayers)
        {
            // check: within circle close to groundPoint, is there any ground
            // .2f is a good value 
            return Physics2D.OverlapCircle(groundPoint.position, .2f, groundLayers);
        }
    }
}