using System;
using BaseUtil.Base;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Player.Domain
{
    public class PlayerAttribute
    {
        public int playerId;
        public bool isAlive; // not alive if killed
        public float moveSpeed = 8f; // good value by experience
        public float jumpForce = 22f; // good value by experience
        public float gravityMultiplier = 3.5f; // combined with 20f jump force, to avoid character being floaty
        public float playerToGroundDistance = 1f; // Not visible, so need to create an empty object on the UI, and calculate the distance to adjust
        public WeaponType weaponType;
        public int skinId;

        public int maxHp = 5;
        public int currentHp;

        public Transform inGameTransform;

        public static PlayerAttribute CreateEmpty(int playerId)
        {
            PlayerAttribute item = new PlayerAttribute
            {
                playerId = playerId,
                skinId = 0,
                isAlive = true,
                weaponType = WeaponType.BASIC,
            };
            item.currentHp = item.maxHp;
            return item;
        }

        public PlayerAttribute Reset()
        {
            isAlive = true;
            weaponType = WeaponType.BASIC;
            currentHp = maxHp;
            return this;
        }

        public void TakeDamage(int damage, Action killedFn)
        {
            currentHp = FnVal.Clamp((currentHp - damage), 0, maxHp);
            if (currentHp == 0)
            {
                isAlive = false;
                killedFn();
            }
        }
    }
}