using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseUtil.GameUtil
{
    public class ShotPointHandler2D
    {
        static float relativeX = 1.2f; // shot point relativeX to player
        static float relativeY = 0.2f; // shot point relativeY to player
        static float relativeTop = 1f; // when pointing up
        static float relativeBottom = -2f; // when pointing down

        public static Transform UpdateShotPoint(Transform shotPoint, Vector3 playerPosition, UserInput userInput, bool isOnGround)
        {
            if (userInput.IsStraightUp()) shotPoint.position = Up(playerPosition);
            if (userInput.IsStraightDown() && !isOnGround) shotPoint.position = Down(playerPosition);
            if (userInput.IsStraightDown() && isOnGround) shotPoint.position = DownOnGround(playerPosition);
            if (userInput.IsStraightLeft()) shotPoint.position = Left(playerPosition);
            if (userInput.IsStraightRight()) shotPoint.position = Right(playerPosition);

            if (userInput.IsUpLeft()) shotPoint.position = UpLeft(playerPosition);
            if (userInput.IsUpRight()) shotPoint.position = UpRight(playerPosition);
            if (userInput.IsDownLeft()) shotPoint.position = DownLeft(playerPosition);
            if (userInput.IsDownRight()) shotPoint.position = DownRight(playerPosition);

            if (userInput.IsNoDirection())
            {
                // put the gun to normal position left/right if no direction key is hit
                shotPoint.position = new Vector3(shotPoint.position.x, (playerPosition.y + relativeY), shotPoint.position.z);
            }

            return shotPoint;
        }

        public static Vector3 Right(Vector3 playerPosition)
        {
            return new Vector3((playerPosition.x + relativeX), (playerPosition.y + relativeY), playerPosition.z);
        }

        public static Vector3 Left(Vector3 playerPosition)
        {
            return new Vector3((playerPosition.x - relativeX), (playerPosition.y + relativeY), playerPosition.z);
        }

        public static Vector3 Up(Vector3 playerPosition)
        {
            return new Vector3(playerPosition.x, (playerPosition.y + relativeY + relativeTop), playerPosition.z);
        }

        public static Vector3 Down(Vector3 playerPosition)
        {
            return new Vector3(playerPosition.x, (playerPosition.y + relativeY + relativeBottom), playerPosition.z);
        }

        public static Vector3 DownOnGround(Vector3 playerPosition)
        {
            return new Vector3(playerPosition.x, (playerPosition.y + relativeY + relativeBottom / 2), playerPosition.z);
        }

        public static Vector3 UpLeft(Vector3 playerPosition)
        {
            return new Vector3((playerPosition.x - (relativeX)), (playerPosition.y + relativeY + (relativeTop / 2)), playerPosition.z);
        }

        public static Vector3 UpRight(Vector3 playerPosition)
        {
            return new Vector3((playerPosition.x + (relativeX)), (playerPosition.y + relativeY + (relativeTop / 2)), playerPosition.z);
        }

        public static Vector3 DownLeft(Vector3 playerPosition)
        {
            return new Vector3((playerPosition.x - (relativeX)), (playerPosition.y + relativeY + (relativeBottom / 2)), playerPosition.z);
        }

        public static Vector3 DownRight(Vector3 playerPosition)
        {
            return new Vector3((playerPosition.x + (relativeX)), (playerPosition.y + relativeY + (relativeBottom / 2)), playerPosition.z);
        }
    }
}