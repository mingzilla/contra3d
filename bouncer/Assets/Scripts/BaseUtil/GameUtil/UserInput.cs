using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BaseUtil.GameUtil
{
    public class UserInput
    {
        public int playerId;
        public float horizontal; // flexible value between 0 and e.g. 0.9f
        public float fixedHorizontal; // fixed to 1
        public bool left;
        public bool right;

        public float vertical;
        public float fixedVertical; // fixed to 1
        public bool up;
        public bool down;

        public bool jump;
        public bool jumpCancelled; // this is to support holding jump button to jump high - basically when cancelled, change to half y velocity 
        public bool fire1;
        public bool fire2;
        public bool fire3;

        public static UserInput Create(int playerId)
        {
            UserInput userInput = new UserInput
            {
                playerId = playerId,
            };
            return UpdateAxes(userInput, Vector2.zero);
        }

        public static UserInput UpdateAxes(UserInput userInput, Vector2 axes1)
        {
            userInput.horizontal = axes1.x;
            userInput.fixedHorizontal = RoundAxisValue(userInput.horizontal);
            userInput.left = Left(userInput.fixedHorizontal, false);
            userInput.right = Right(userInput.fixedHorizontal, false);

            userInput.vertical = axes1.y;
            userInput.fixedVertical = RoundAxisValue(userInput.vertical);
            userInput.up = Up(userInput.fixedVertical, false);
            userInput.down = Down(userInput.fixedVertical, false);

            return userInput;
        }

        public bool IsStraightUp()
        {
            return up && !down && !left && !right;
        }

        public bool IsStraightDown()
        {
            return !up && down && !left && !right;
        }

        public bool IsStraightLeft()
        {
            return !down && !up && left && !right;
        }

        public bool IsStraightRight()
        {
            return !down && !up && !left && right;
        }

        public bool IsUpLeft()
        {
            return Up(vertical, true) && !down && Left(horizontal, true) && !right;
        }

        public bool IsUpRight()
        {
            return Up(vertical, true) && !down && !left && Right(horizontal, true);
        }

        public bool IsDownLeft()
        {
            return !up && Down(vertical, true) && Left(horizontal, true) && !right;
        }

        public bool IsDownRight()
        {
            return !up && Down(vertical, true) && !left && Right(horizontal, true);
        }

        public bool IsNoDirection()
        {
            return !up && !down && !left && !right;
        }

        private static bool Left(float horizontal, bool multiInputs)
        {
            if (!multiInputs) return horizontal < -0.9f;
            return horizontal < -0.3f;
        }

        private static bool Right(float horizontal, bool multiInputs)
        {
            if (!multiInputs) return horizontal > 0.9f;
            return horizontal > 0.3f;
        }

        private static bool Up(float vertical, bool multiInputs)
        {
            if (!multiInputs) return vertical > 0.9f;
            return vertical > 0.3f;
        }

        private static bool Down(float vertical, bool multiInputs)
        {
            if (!multiInputs) return vertical < -0.9f;
            return vertical < -0.3f;
        }

        private static float RoundAxisValue(float value)
        {
            if (value > 0.3f) return 1f;
            if (value < -0.3f) return -1f;
            return 0f;
        }

        public static UserInput Move(UserInput userInput, InputAction.CallbackContext context)
        {
            Vector2 axes1 = context.ReadValue<Vector2>();
            return UpdateAxes(userInput, axes1);
        }

        public static readonly Action<UserInput> ResetTriggers = (userInput) =>
        {
            userInput.jump = false;
            userInput.jumpCancelled = false;
            userInput.fire1 = false;
            userInput.fire2 = false;
            userInput.fire3 = false;
        };
    }
}