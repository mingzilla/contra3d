using UnityEngine.InputSystem;

namespace ProjectContent.Scripts.Player
{
    public interface IControllable
    {
        public void InputMove(InputAction.CallbackContext context);
        public void KeySelect(InputAction.CallbackContext context);
        public void KeyStart(InputAction.CallbackContext context);
        public void KeyA(InputAction.CallbackContext context);
        public void KeyB(InputAction.CallbackContext context);
        public void KeyX(InputAction.CallbackContext context);
        public void KeyY(InputAction.CallbackContext context);
        public void KeyPadUp(InputAction.CallbackContext context);
        public void KeyPadDown(InputAction.CallbackContext context);
        public void KeyPadLeft(InputAction.CallbackContext context);
        public void KeyPadRight(InputAction.CallbackContext context);
        public void KeyLB(InputAction.CallbackContext context);
        public void KeyRB(InputAction.CallbackContext context);
        public void KeyLT(InputAction.CallbackContext context);
        public void KeyRT(InputAction.CallbackContext context);
        public void KeyboardAnyKey(InputAction.CallbackContext context);
    }
}