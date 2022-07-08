using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemySpiderWeakPointController : AbstractDestructibleController
    {
        public int hp = 100;

        void Start()
        {
            gameObject.layer = GameLayer.ENEMY.GetLayer();
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(AppResource.instance.enemyDestroyedSmallExplosion, position, 1f);
            AppSfx.Play(AppSfx.instance.bigEnemyDamaged);
            hp -= damage;
            if (hp <= 0)
            {
                isBroken = true;
            }
        }

        public override float GetDetectionRange()
        {
            return -1;
        }
    }
}