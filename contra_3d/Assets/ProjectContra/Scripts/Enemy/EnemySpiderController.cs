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
    public class EnemySpiderController : AbstractRangeDetectionController
    {
        public int damage = 1;
        public int moveSpeed = 4;
        public float detectionRange = 40f;
        public int directionChangeTime = 5;

        private bool isActive = false;
        private EnemySpiderWeakPointController weakPointCtrl;
        private GameStoreData storeData;
        private bool moveXLeft = false; // this 3D model is not 180 rotated, so it runs oppositely
        private Animator animatorCtrl;
        private IntervalState intervalState;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.INVISIBLE_WALL_TO_PLAYER.GetLayer();
            weakPointCtrl = gameObject.GetComponentInChildren<EnemySpiderWeakPointController>();
            animatorCtrl = gameObject.GetComponent<Animator>();
            animatorCtrl.enabled = false;
            intervalState = IntervalState.Create(directionChangeTime);
        }

        void Update()
        {
            if (!storeData.HasPlayer()) return;
            Transform closestPlayer = storeData.GetClosestPlayer(transform.position).inGameTransform;
            if (!isActive && UnityFn.IsInRange(transform, closestPlayer, GetDetectionRange()))
            {
                isActive = true;
                animatorCtrl.enabled = true;
            }
            if (isActive) UnityFn.RunWithInterval(AppResource.instance, intervalState, () => moveXLeft = !moveXLeft);
            if (isActive) MovementUtil.MoveX(transform, (moveXLeft ? -1 : 1), moveSpeed);
            if (weakPointCtrl.isBroken) Explode();
        }

        private void Explode()
        {
            UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 3f);
            AppSfx.PlayRepeatedly(AppSfx.instance.bigEnemyDeath, 3);
            Destroy(gameObject);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}