using System.Collections.Generic;
using System.Linq;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.EnemyBullet;
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
        [SerializeField] private GameObject invisibleWall;
        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject afterwardsCamera;

        private bool isActive = false;
        private EnemySpiderWeakPointController weakPointCtrl;
        private GameStoreData storeData;
        private bool moveXLeft = false; // this 3D model is not 180 rotated, so it runs oppositely
        private Animator animatorCtrl;
        private IntervalState changeDirectionInterval;
        private IntervalState shotIntervalState = IntervalState.Create(2f);

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.INVISIBLE_WALL_TO_PLAYER.GetLayer();
            weakPointCtrl = gameObject.GetComponentInChildren<EnemySpiderWeakPointController>();
            animatorCtrl = gameObject.GetComponent<Animator>();
            animatorCtrl.enabled = false;
            changeDirectionInterval = IntervalState.Create(directionChangeTime);
        }

        void Update()
        {
            if (!storeData.HasPlayer()) return;
            Transform closestPlayer = storeData.GetClosestPlayer(transform.position).inGameTransform;
            if (!isActive && UnityFn.IsInRange(transform, closestPlayer, GetDetectionRange()))
            {
                isActive = true;
                animatorCtrl.enabled = true;
                AppMusic.instance.PlayLv3MidLevelMusic();
                gameCamera.SetActive(false);
                afterwardsCamera.SetActive(true);
            }
            if (isActive)
            {
                UnityFn.RunWithInterval(AppResource.instance, changeDirectionInterval, () => moveXLeft = !moveXLeft);
                MovementUtil.MoveX(transform, (moveXLeft ? -1 : 1), moveSpeed);
                FireShots(transform.position);

            }
            if (weakPointCtrl.isBroken) Explode();
        }

        void FireShots(Vector3 position)
        {
            UnityFn.RunWithInterval(AppResource.instance, shotIntervalState, () =>
            {
                EnemyBasicBulletController.Spawn(position + new Vector3(-2f, 0.5f, 0f), position + new Vector3(-3f, 0.8f, 0f), EnemyBulletType.BASIC, false);
                EnemyBasicBulletController.Spawn(position + new Vector3(-2f, -0.5f, 0f), position + new Vector3(-3f, -0.8f, 0f), EnemyBulletType.BASIC, false);
                EnemyBasicBulletController.Spawn(position + new Vector3(2f, 0.5f, 0f), position + new Vector3(3f, 0.8f, 0f), EnemyBulletType.BASIC, false);
                EnemyBasicBulletController.Spawn(position + new Vector3(2f, -0.5f, 0f), position + new Vector3(3f, -0.8f, 0f), EnemyBulletType.BASIC, false);
                EnemyCurvedBulletController.Spawn(position + new Vector3(-2f, 0f, 0f), EnemyBulletType.CURVED, true);
                EnemyCurvedBulletController.Spawn(position + new Vector3(2f, 0f, 0f), EnemyBulletType.CURVED, false);
            });
        }

        private void Explode()
        {
            UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 3f);
            AppSfx.PlayRepeatedly(AppSfx.instance.bigEnemyDeath, 3);
            Destroy(invisibleWall);
            Destroy(gameObject);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}