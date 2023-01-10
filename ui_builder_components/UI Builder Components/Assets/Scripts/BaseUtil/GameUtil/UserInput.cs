using BaseUtil.Base;
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

        public bool dash;

        public bool fire1;
        public bool fire2;
        public bool fire3;

        public bool swing; // e.g. swing a sword. this reduces movement speed on ground, speed should not be affected in the air e.g. when jumping
        public bool magicTop;
        public bool magicBottom;
        public bool magicLeft;
        public bool magicRight;

        public bool isHoldingLb;
        public bool isHoldingLt;
        public bool isHoldingRb;
        public bool isHoldingRt;

        public static void ResetTriggers(UserInput userInput)
        {
            // don't reset up,down,left,right. let velocity stop itself
            userInput.jump = false;
            userInput.jumpCancelled = false;
            userInput.dash = false;
            userInput.fire1 = false;
            userInput.fire2 = false;
            userInput.fire3 = false;
            userInput.swing = false;
            userInput.magicTop = false;
            userInput.magicBottom = false;
            userInput.magicLeft = false;
            userInput.magicRight = false;
        }

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

        public bool IsMovingHorizontally()
        {
            return fixedHorizontal != 0f;
        }

        public bool IsMoving()
        {
            return (fixedHorizontal != 0f) || (fixedVertical != 0f);
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
            return FnVal.RoundAxisValue(value, 0.3f);
        }

        public static UserInput Move(UserInput userInput, InputAction.CallbackContext context)
        {
            Vector2 axes1 = context.ReadValue<Vector2>();
            return UpdateAxes(userInput, axes1);
        }

        public static bool IsFacingRight(bool isFacingForward, UserInput userInput)
        {
            if (userInput.left) return false;
            if (userInput.right) return true;
            return isFacingForward;
        }

        public static bool IsFacingUp(bool isFacingForward, UserInput userInput)
        {
            if (userInput.down) return false;
            if (userInput.up) return true;
            return isFacingForward;
        }

        public static bool IsMenuKeyboardOk()
        {
            return Keyboard.current.jKey.wasPressedThisFrame;
        }

        public static bool IsMenuKeyboardCancel()
        {
            return Keyboard.current.kKey.wasPressedThisFrame;
        }

        public static bool IsMenuGamepadOk()
        {
            return Gamepad.current.buttonSouth.wasPressedThisFrame;
        }

        public static bool IsMenuGamepadCancel()
        {
            return Gamepad.current.buttonEast.wasPressedThisFrame;
        }

        public bool IsUsingMagic()
        {
            return magicTop || magicBottom || magicLeft || magicRight;
        }
    }
}