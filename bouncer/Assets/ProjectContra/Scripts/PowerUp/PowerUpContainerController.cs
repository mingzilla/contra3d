using System.Collections.Generic;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.PowerUp
{
    public class PowerUpContainerController : AbstractDestructibleController
    {
        public int moveSpeed = 10;
        public float detectionRange = 60f;
        public bool isActive = false;

        private GameStoreData storeData;
        private Rigidbody rb;
        private MeshRenderer meshRenderer;
        private GameObject destroyEffect;
        private Transform closestPlayer;
        private Vector3 targetPosition = Vector3.zero;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.POWER_UP_CONTAINER.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, false, true);
            UnityFn.AddSphereCollider(gameObject, 1f, false);
            meshRenderer = UnityFn.MakeInvisible(gameObject);
            destroyEffect = AppResource.instance.powerUpDestroyedSmallExplosion;
        }

        void Update()
        {
            if (!storeData.HasPlayer()) return;
            if (closestPlayer == null) closestPlayer = storeData.GetClosestPlayer(transform.position).inGameTransform;
            if (targetPosition == Vector3.zero) targetPosition = closestPlayer.position;
            if (!isActive && UnityFn.IsInRange(transform, closestPlayer, GetDetectionRange()))
            {
                isActive = true;
                meshRenderer.enabled = true;
            }
            if (isActive) MovementUtil.MoveTowardsPositionX3D(transform, targetPosition, moveSpeed, delta => targetPosition += delta);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            Destroy(gameObject);
        }
    }
}