using System;
using BaseUtil.Base;
using UnityEngine;

namespace ProjectContent.Scripts.Data
{
    public class PlayerAttribute
    {
        public int playerId;
        public float moveSpeed = 6f;
        public float jumpForce = 15f;
        public float dashForce = 15f;
        public float gravityMultiplier = 3.5f; // combined with jump force, to avoid character being floaty
        public float playerToGroundDistance = 0f; // Not visible, so need to create an empty object on the UI, and calculate the distance to adjust
        public int maxHp = 8;

        public int skinId;

        public bool isAlive; // not alive if killed
        public int currentHp;

        public PlayerSkillAllocation skillAllocation = new PlayerSkillAllocation();
        
        public Transform inGameTransform;

        public static PlayerAttribute CreateEmpty(int playerId)
        {
            PlayerAttribute item = new()
            {
                playerId = playerId,
                skinId = 0,
                isAlive = true
            };
            item.currentHp = item.maxHp;
            return item;
        }

        public PlayerAttribute Reset()
        {
            isAlive = true;
            currentHp = maxHp;
            return this;
        }

        public void TakeDamage(int damage, Action killedFn)
        {
            currentHp = FnVal.Clamp((currentHp - damage), 0, maxHp);
            if (currentHp == 0 && isAlive)
            {
                isAlive = false;
                killedFn();
            }
        }
    }
}