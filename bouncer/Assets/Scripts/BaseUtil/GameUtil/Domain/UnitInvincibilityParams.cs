using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Types;
using UnityEngine;

namespace BaseUtil.GameUtil.Domain
{
    public class UnitInvincibilityParams
    {
        public UnitInvincibility unitInvincibility;
        public List<SpriteRenderer> spriteRenderers;
        public float invincibilityCounter = 0f;
        public float flashCounter = 0f;

        private bool isStillInvincible = false;

        public static UnitInvincibilityParams Create(string invincibilityType, Transform unitTransform, string tagName)
        {
            UnitInvincibility unitInvincibility = UnitInvincibility.GetByNameWithDefault(invincibilityType);
            List<SpriteRenderer> spriteRenderers = UnityFn.FindComponentsFromChildrenWithTag<SpriteRenderer>(unitTransform, tagName);

            if (unitInvincibility.CanFlash() && spriteRenderers.Count == 0)
            {
                Debug.LogError("Missing Tag. To enable flashing, UnitStat needs to have at least one direct child Sprite GameObject, Sprite GameObject needs to be tagged as Sprite");
            }

            return new UnitInvincibilityParams
            {
                unitInvincibility = unitInvincibility,
                spriteRenderers = spriteRenderers,
            };
        }

        public static UnitInvincibilityParams UpdateValuesAndHandleFlashing(UnitInvincibilityParams currentParams, float deltaTime)
        {
            currentParams.invincibilityCounter = FnVal.AtLeastF(0f, currentParams.invincibilityCounter - deltaTime);
            currentParams.flashCounter = FnVal.AtLeastF(0f, currentParams.flashCounter - deltaTime);
            currentParams = HandleFlashing(currentParams);

            return currentParams;
        }

        public void TriggerInvincibility()
        {
            if (isStillInvincible) return; // if is still invincible, don't change duration, otherwise if you keep hitting, unit keeps extending invincibility
            invincibilityCounter = unitInvincibility.invincibilityDuration;
            isStillInvincible = true;
        }

        public bool IsInvincible()
        {
            return invincibilityCounter > 0;
        }

        /**
         * @return updated flashCounter
         */
        public static UnitInvincibilityParams HandleFlashing(UnitInvincibilityParams currentParams)
        {
            UnitInvincibilityParams updatedParams = currentParams;
            UnitInvincibility unitInvincibility = currentParams.unitInvincibility;

            if (currentParams.invincibilityCounter > 0)
            {
                if (unitInvincibility.CanFlash() && currentParams.flashCounter == 0)
                {
                    updatedParams.flashCounter = currentParams.unitInvincibility.flashInterval;
                    UnityFn.ToggleSpriteRenderer(currentParams.spriteRenderers);
                }
                updatedParams.isStillInvincible = true;
            }
            else
            {
                updatedParams.flashCounter = 0f;
                if (unitInvincibility.CanFlash() && updatedParams.isStillInvincible)
                {
                    UnityFn.EnableSpriteRenderer(currentParams.spriteRenderers);
                }
                updatedParams.isStillInvincible = false;
            }

            return updatedParams;
        }
    }
}