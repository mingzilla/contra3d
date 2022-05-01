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
        public float yPositionDelta = 1f;

        private GameStoreData storeData;
        private Rigidbody rb;
        private MeshRenderer meshRenderer;
        private GameObject destroyEffect;
        private Transform closestPlayer;
        private int xMovementValue = 0;
        private float yLow;
        private float yHigh;
        private bool isGoingUp = true;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.POWER_UP_CONTAINER.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, false, true);
            UnityFn.AddSphereCollider(gameObject, 1f, false);
            meshRenderer = UnityFn.MakeInvisible(gameObject);
            destroyEffect = AppResource.instance.powerUpDestroyedSmallExplosion;
            float originalY = transform.position.y;
            yLow = originalY - yPositionDelta;
            yHigh = originalY + yPositionDelta;
        }

        void Update()
        {
            if (!storeData.HasPlayer()) return;
            Vector3 position = transform.position;
            if (closestPlayer == null) closestPlayer = storeData.GetClosestPlayer(position).inGameTransform;
            xMovementValue = (xMovementValue != 0) ? xMovementValue : (closestPlayer.position.x > position.x ? 1 : -1);
            if (!isActive && UnityFn.IsInRange(transform, closestPlayer, GetDetectionRange()))
            {
                isActive = true;
                meshRenderer.enabled = true;
            }
            if (isActive)
            {
                MovementUtil.MoveX(transform, xMovementValue, moveSpeed);
                if (position.y > yHigh) isGoingUp = false;
                if (position.y < yLow) isGoingUp = true;
                transform.position += new Vector3(0f, (moveSpeed / 2f * Time.deltaTime), 0f) * (isGoingUp ? 1 : -1);
            }
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