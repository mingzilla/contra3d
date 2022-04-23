using UnityEngine;

namespace ProjectContra.Scripts.Player.domain
{
    public class PlayerAttribute
    {
        public int playerId;
        public float moveSpeed = 8; // good value by experience
        public float jumpForce = 20; // good value by experience
        public float dashSpeed = 25; // good value by experience
        public float dashTime = 0.2f; // good value by experience

        public bool hasDoubleJumpAbility = true;
        public bool hasDashAbility = true;
        public bool hasSmallShapeAbility = true;

        public Vector3 playerPosition = Vector3.zero;

        public static PlayerAttribute CreateEmpty(int playerId)
        {
            return new PlayerAttribute
            {
                playerId = playerId,
            };
        }

        public PlayerAttribute Reset()
        {
            return CreateEmpty(playerId);
        }
    }
}