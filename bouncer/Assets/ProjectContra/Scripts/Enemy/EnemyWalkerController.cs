using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyWalkerController : EnemyController
    {
        public int damage = 1;
        public int moveSpeed = 10;
        public float jumpForce = 20f;
        public float detectionRange = 60f;
        public float delayToBeActive = 1f;
        public bool isActive = false;

        private GameStoreData storeData;
        private Rigidbody rb;
        private MeshRenderer meshRenderer;
        private CapsuleCollider theCollider;
        private LayerMask destructibleLayers;
        private GameObject destroyEffect;
        private Transform closestPlayer;
        private Vector3 targetPosition = Vector3.zero;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidBodyAndFreezeZ(gameObject);
            meshRenderer = UnityFn.MakeInvisible(gameObject);
            destructibleLayers = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});
            destroyEffect = AppResource.instance.enemyDestroyedSmallExplosion;
        }

        void Update()
        {
            if (!storeData.HasPlayer()) return;
            if (closestPlayer == null) closestPlayer = storeData.GetClosestPlayer(transform.position).inGameTransform;
            if (targetPosition == Vector3.zero) targetPosition = closestPlayer.position;
            if (UnityFn.IsInRange(transform, closestPlayer, GetDetectionRange()))
            {
                if (!isActive)
                    UnityFn.SetTimeout(this, delayToBeActive, () =>
                    {
                        isActive = true;
                        meshRenderer.enabled = true;
                        transform.LookAt(closestPlayer);
                    });
            }
            if (isActive) MovementUtil.MoveTowardsPosition3D(transform, targetPosition, moveSpeed, delta => targetPosition += delta);
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
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER))
            {
                Vector3 location = transform.position;
                GameFn.DealDamage(location, 2, destructibleLayers, (obj) =>
                {
                    CharacterInGameController character = obj.GetComponent<CharacterInGameController>();
                    character.TakeDamage(location, damage);
                });
            }
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