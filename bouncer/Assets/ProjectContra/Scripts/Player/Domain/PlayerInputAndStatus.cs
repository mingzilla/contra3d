using UnityEngine.InputSystem;

namespace ProjectContra.Scripts.Player.Domain
{
    public class PlayerInputAndStatus
    {
        public PlayerInput playerInput;
        public bool isReady;

        public static PlayerInputAndStatus Create(PlayerInput playerInput)
        {
            return new PlayerInputAndStatus()
            {
                playerInput = playerInput,
                isReady = false
            };
        }
    }
}