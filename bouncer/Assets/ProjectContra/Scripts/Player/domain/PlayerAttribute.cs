using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Player.domain
{
    public class PlayerAttribute
    {
        public int playerId;
        public bool isAlive; // not alive if killed
        public float moveSpeed = 8f; // good value by experience
        public float jumpForce = 20f; // good value by experience
        public float gravityMultiplier = 3.5f; // combined with 20f jump force, to avoid character being floaty
        public float playerToGroundDistance = 1f; // Not visible, so need to create an empty object on the UI, and calculate the distance to adjust
        public WeaponType weaponType;

        public int maxHp;
        public int currentHp;

        public Transform inGameTransform;

        public static PlayerAttribute CreateEmpty(int playerId)
        {
            return new PlayerAttribute
            {
                playerId = playerId,
                isAlive = true,
                weaponType = WeaponType.BASIC,
            };
        }

        public PlayerAttribute Reset()
        {
            return CreateEmpty(playerId);
        }
    }
}