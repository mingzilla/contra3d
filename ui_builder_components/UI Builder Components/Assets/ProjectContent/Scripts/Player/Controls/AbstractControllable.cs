using BaseUtil.GameUtil;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectContent.Scripts.Player.Controls
{
    public abstract class AbstractControllable : IControllable
    {
        protected UserInput userInput;
        protected GameObject gameObject;
        protected Transform transform;
        protected PlayerMono mono;
        protected int playerId;

        public static T BaseCreate<T>(PlayerMono mono, int playerId) where T : AbstractControllable, new()
        {
            GameObject gameObject = mono.gameObject;

            return new T()
            {
                mono = mono,
                gameObject = gameObject,
                transform = gameObject.transform,
                playerId = playerId,
                userInput = UserInput.Create(playerId),
            };
        }

        protected abstract void Start();
        public abstract void FixedUpdate();

        public abstract void InputMove(InputAction.CallbackContext context);
        public abstract void KeySelect(InputAction.CallbackContext context);
        public abstract void KeyStart(InputAction.CallbackContext context);
        public abstract void KeyA(InputAction.CallbackContext context);
        public abstract void KeyB(InputAction.CallbackContext context);
        public abstract void KeyX(InputAction.CallbackContext context);
        public abstract void KeyY(InputAction.CallbackContext context);
        public abstract void KeyPadUp(InputAction.CallbackContext context);
        public abstract void KeyPadDown(InputAction.CallbackContext context);
        public abstract void KeyPadLeft(InputAction.CallbackContext context);
        public abstract void KeyPadRight(InputAction.CallbackContext context);
        public abstract void KeyLB(InputAction.CallbackContext context);
        public abstract void KeyRB(InputAction.CallbackContext context);
        public abstract void KeyLT(InputAction.CallbackContext context);
        public abstract void KeyRT(InputAction.CallbackContext context);
        public abstract void KeyboardAnyKey(InputAction.CallbackContext context);
    }
}