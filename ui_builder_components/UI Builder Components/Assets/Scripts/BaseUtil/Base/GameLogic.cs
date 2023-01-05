using System.Collections;
using System.Collections.Generic;
using System;

namespace BaseUtil.Base
{
    public static class GameLogic
    {
        /**
         * @return new isInDoubleJumpStatus
         */
        public static bool HandleJumpFromGroundOrDoubleJump(bool isOnGround, bool isInDoubleJumpStatus, Action jumpFn)
        {
            if (isOnGround)
            {
                jumpFn();
                return false;
            }
            else if (!isInDoubleJumpStatus)
            {
                jumpFn();
                return true;
            }
            else
            {
                return true;
            }
        }
    }
}