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
    public static class GameFn
    {
        /**
         * @param blastRange generally needs to be 2. Too small may not overlap with other layer so may not trigger damage.
         */
        public static void DealDamage(Vector3 position, float blastRange, LayerMask affectedLayers, Action<GameObject> damagingFn)
        {
            Collider[] affectedObjects = Physics.OverlapSphere(position, blastRange, affectedLayers, QueryTriggerInteraction.Ignore);
            foreach (var item in affectedObjects)
            {
                damagingFn(item.gameObject);
            }
        }

        /// <param name="playerPosition"></param>
        /// <param name="playerToGroundDistance">Not visible, so need to create an empty object on the UI, and calculate the distance to adjust</param>
        /// <param name="groundLayers"></param>
        /// <returns></returns>
        public static bool IsOnGround(Vector3 playerPosition, float playerToGroundDistance, LayerMask groundLayers)
        {
            Vector3 position = new Vector3(playerPosition.x, playerPosition.y - playerToGroundDistance, playerPosition.z);
            // check: within circle close to groundPoint, is there any ground
            // .2f is a good value 
            return Physics.CheckSphere(position, .2f, groundLayers);
        }
    }
}