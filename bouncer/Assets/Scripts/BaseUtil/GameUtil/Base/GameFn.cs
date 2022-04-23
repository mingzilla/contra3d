using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseUtil.Base;
using BaseUtil.GameUtil.Types;
using BaseUtil.GameUtil.Domain;
using System;

namespace BaseUtil.GameUtil.Base
{
    /**
     * Common things a game needs to do.
     */
    public static class GameFn
    {
        public static void DealDamage(Vector3 position, float blastRange, LayerMask affectedLayers, Action<GameObject> damagingFn)
        {
            Collider2D[] affectedObjects = Physics2D.OverlapCircleAll(position, blastRange, affectedLayers);
            foreach (var item in affectedObjects)
            {
                damagingFn(item.gameObject);
            }
        }

        /**
         * @param blastRange generally needs to be 2. Too small may not overlap with other layer so may not trigger damage.
         */
        public static void DealDamageToUnit(Vector3 position, float blastRange, LayerMask affectedLayers, int damage, Action<UnitStat> updateUnitStatFn)
        {
            GameFn.DealDamage(position, blastRange, affectedLayers, (GameObject gameObject) =>
            {
                GameObject rootObject = UnityFn.GetUnitRootOrSelf(gameObject);
                if (rootObject == null)
                {
                    Debug.LogError("Can only deal damage to object that is a Unit Root or inside a Unit Root.");
                    return;
                }
                UnitStat stat = rootObject.GetComponent<UnitStat>();
                if (stat == null)
                {
                    // UnitStat needs to attatch to root because there can be siblings sprites share the same stats
                    // However, the active sprit is the location for explosion effect to occur
                    Debug.LogError("UnitStat script is not attached to an object tagged as 'Unit Root'");
                    return;
                }
                stat.TakeDamage(damage, gameObject.transform.position);
                updateUnitStatFn(stat);
            });
        }
    }
}