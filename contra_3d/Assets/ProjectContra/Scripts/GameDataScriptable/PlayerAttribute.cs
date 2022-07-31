using System;
using BaseUtil.Base;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.GameDataScriptable
{
    [CreateAssetMenu(menuName = "Game State/PlayerAttribute", fileName = "PlayerAttribute")]
    public class PlayerAttribute : ScriptableObject
    {
        public int playerId;
        public bool isAlive; // not alive if killed
        public float moveSpeed = 8f; // good value by experience
        public float jumpForce = 22f; // good value by experience
        public float gravityMultiplier = 3.5f; // combined with 20f jump force, to avoid character being floaty
        public float playerToGroundDistance = 0f; // Not visible, so need to create an empty object on the UI, and calculate the distance to adjust
        public WeaponType weaponType = DEFAULT_WEAPON_TYPE;
        public int skinId;

        public int maxHp = 8;
        public int currentHp;

        public Transform inGameTransform;

        private static readonly WeaponType DEFAULT_WEAPON_TYPE = WeaponType.BASIC;

        public static PlayerAttribute CreateEmpty(int playerId)
        {
            PlayerAttribute item = CreateInstance<PlayerAttribute>();
            item.playerId = playerId;
            item.skinId = 0;
            item.isAlive = true;
            item.weaponType = DEFAULT_WEAPON_TYPE;
            item.currentHp = item.maxHp;
            return item;
        }

        public PlayerAttribute Reset()
        {
            isAlive = true;
            weaponType = DEFAULT_WEAPON_TYPE;
            currentHp = maxHp;
            return this;
        }

        public void TakeDamage(int damage, Action killedFn)
        {
            currentHp = FnVal.Clamp((currentHp - damage), 0, maxHp);
            weaponType = DEFAULT_WEAPON_TYPE;
            if (currentHp == 0 && isAlive)
            {
                isAlive = false;
                killedFn();
            }
        }
    }
}