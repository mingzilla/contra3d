using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Types;

namespace BaseUtil.GameUtil.Domain
{
    public class UnitStat : MonoBehaviour
    {
        public string identifier;
        public GameObject destroyEffect;

        public HealthBar healthBar;
        public int maxHp;
        public int currentHp;

        public string invincibilityType;
        private UnitInvincibilityParams unitInvincibilityParams;

        private void Start()
        {
            currentHp = maxHp;
            if (healthBar != null) healthBar.SetMaxHealth(maxHp);
            unitInvincibilityParams = UnitInvincibilityParams.Create(invincibilityType, transform);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            unitInvincibilityParams = UnitInvincibilityParams.UpdateValuesAndHandleFlashing(unitInvincibilityParams, deltaTime);
        }

        public void TakeDamage(int damage, Vector3 destroyEffectPosition)
        {
            if (!unitInvincibilityParams.IsInvincible())
            {
                currentHp = FnVal.AtLeast(0, currentHp - damage);
                if (healthBar != null) healthBar.SetHealth(currentHp);
            }

            if (currentHp == 0)
            {
                UnityFn.CreateEffect(destroyEffect, destroyEffectPosition, 1f);
                if (GameLayer.Matches(gameObject.layer, GameLayer.PLAYER))
                {
                    UnityFn.SetActiveOnTaggedRootOrSelf(gameObject, false);
                }
                else
                {
                    UnityFn.DestroyTaggedRootOrSelf(gameObject);
                }
            }
            else
            {
                unitInvincibilityParams.TriggerInvincibility();
            }
        }
    }
}