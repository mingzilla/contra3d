using UnityEngine.InputSystem;

namespace BaseUtil.GameUtil.PlayerManagement
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