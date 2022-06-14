using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyWalkerController : AbstractDestructibleController
    {
        public int damage = 1;
        public int moveSpeed = 8;
        public float jumpForce = 10f;
        public float detectionRange = 60f;
        private bool isActive = false;

        private GameStoreData storeData;
        private Rigidbody rb;
        private CapsuleCollider theCollider;
        private LayerMask destructibleLayers;
        private GameObject destroyEffect;
        private bool moveXLeft = false;
        private Animator animatorCtrl;
        private static readonly int isRunning = Animator.StringToHash("isRunning");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, true, true);
            destructibleLayers = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
            animatorCtrl = gameObject.GetComponent<Animator>();
        }

        void Update()
        {
            if (!storeData.HasPlayer()) return;
            Move();
        }

        void Move()
        {
            Transform closestPlayer = storeData.GetClosestPlayer(transform.position).inGameTransform;
            if (!isActive && UnityFn.IsInRange(transform, closestPlayer, GetDetectionRange()))
            {
                isActive = true;
                moveXLeft = closestPlayer.position.x < transform.position.x;
                animatorCtrl.SetBool(isRunning, true);
                transform.rotation = UnityFn.LookXZ(transform, closestPlayer);
            }
            bool moveXZ = AppResource.instance.GetCurrentSceneInitData().moveXZ;
            if (isActive && !moveXZ) MovementUtil.MoveX(transform, (moveXLeft ? -1 : 1), moveSpeed);
            if (isActive && moveXZ) MovementUtil.FollowXZTowardsPosition3D(transform, closestPlayer, 0.5f, moveSpeed, Time.deltaTime);
        }

        void OnCollisionEnter(Collision other)
        {
            DealDamage(other);
        }

        void OnCollisionStay(Collision other)
        {
            DealDamage(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            JumpAtTriggerPoint(other);
        }

        void DealDamage(Collision other)
        {
            EnemyUtil.DealDamageToPlayer(transform.position, damage, other.collider);
        }

        private void JumpAtTriggerPoint(Collider other)
        {
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.ENEMY_JUMP_POINT))
            {
                GameFn.HandleJump(rb, jumpForce);
            }
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            AppSfx.PlayAdjusted(AppSfx.instance.enemyDeath);
            Destroy(gameObject);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }

        protected void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}