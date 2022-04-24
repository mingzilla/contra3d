using UnityEngine;

namespace ProjectContra.Scripts.Player.domain
{
    public class PlayerAttribute
    {
        public int playerId;
        public float moveSpeed = 8f; // good value by experience
        public float jumpForce = 20f; // good value by experience
        public float dashSpeed = 25f; // good value by experience
        public float dashTime = 0.2f; // good value by experience
        public float playerToGroundDistance = 1f; // Not visible, so need to create an empty object on the UI, and calculate the distance to adjust

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