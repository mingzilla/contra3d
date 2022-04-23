using UnityEngine;

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

        /// <param name="playerPosition"></param>
        /// <param name="playerToGroundDistance">Not visible, so need to create an empty object on the UI, and calculate the distance to adjust</param>
        /// <param name="groundLayers"></param>
        /// <returns></returns>
        public static bool IsOnGround(Vector3 playerPosition, float playerToGroundDistance, LayerMask groundLayers)
        {
            
            Vector3 position = new Vector3(playerPosition.x, playerPosition.y - playerToGroundDistance, playerPosition.z);
            // check: within circle close to groundPoint, is there any ground
            // .2f is a good value 
            return Physics2D.OverlapCircle(position, .2f, groundLayers);
        }
    }
}