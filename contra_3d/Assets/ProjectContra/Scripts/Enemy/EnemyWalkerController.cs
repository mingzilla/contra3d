using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyWalkerController : AbstractDestructibleController
    {
        public int damage = 1;
        public int moveSpeed = 8;
        public float jumpForce = 22f;
        public float detectionRange = 60f;
        public bool isActive = false;

        private GameStoreData storeData;
        private Rigidbody rb;
        private CapsuleCollider theCollider;
        private LayerMask destructibleLayers;
        private GameObject destroyEffect;
        private Transform closestPlayer;
        private int xMovementValue = 0;
        private Animator animatorCtrl;
        private static readonly int isRunning = Animator.StringToHash("isRunning");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, true, true);
            // UnityFn.AddCapsuleCollider(gameObject, 0.5f, 2, false);
            destructibleLayers = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
            animatorCtrl = gameObject.GetComponent<Animator>();
        }

        void Update()
        {
            if (!storeData.HasPlayer()) return;
            if (closestPlayer == null) closestPlayer = storeData.GetClosestPlayer(transform.position).inGameTransform;
            xMovementValue = (xMovementValue != 0) ? xMovementValue : (closestPlayer.position.x > transform.position.x ? 1 : -1);
            if (!isActive && UnityFn.IsInRange(transform, closestPlayer, GetDetectionRange()))
            {
                isActive = true;
                bool isFacingRight = transform.position.x < closestPlayer.position.x;
                UnitDisplayHandler3D.HandleLeftRightFacing(transform, isFacingRight);
            }
            if (isActive)
            {
                animatorCtrl.SetBool(isRunning, true);
                MovementUtil.MoveX(transform, xMovementValue, moveSpeed);
            }
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
            EnemyUtil.DealDamageToPlayer(transform.position, 2, destructibleLayers, damage, other.collider);
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