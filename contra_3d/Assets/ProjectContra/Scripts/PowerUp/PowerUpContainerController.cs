using System;
using BaseUtil.Base;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.GameDataScriptable;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.PowerUp
{
    public class PowerUpContainerController : AbstractDestructibleController
    {
        public int moveSpeed = 10;
        public float detectionRange = 60f;
        public string weaponTypeName = WeaponType.BLAST.name;

        private GameStoreData storeData;
        private Rigidbody rb;
        private MeshRenderer meshRenderer;
        private GameObject destroyEffect;
        private Transform closestPlayer;
        private int xMovementValue = 0;
        private bool hasRunTakeDamage = false;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.POWER_UP_CONTAINER.GetLayer();
            rb = UnityFn.GetOrAddRigidbody(gameObject, false, true);
            UnityFn.AddSphereCollider(gameObject, 1f, false);
            meshRenderer = UnityFn.MakeInvisible<MeshRenderer>(gameObject);
            destroyEffect = AppResource.instance.powerUpDestroyedSmallExplosion;
        }

        void Update()
        {
            TriggerIfPlayerIsInRange(storeData, GetDetectionRange(), closestPlayer =>
            {
                meshRenderer.enabled = true;
                xMovementValue = (xMovementValue != 0) ? xMovementValue : (closestPlayer.position.x > transform.position.x ? 1 : -1);
            });

            if (isTriggered)
            {
                bool moveXZ = AppResource.instance.GetCurrentScene().moveXZ;
                MovementUtil.MoveX(transform, xMovementValue, moveSpeed);
                float movementFactor = MovementUtil.CalculateCircularMovementFactor(1f);
                float movementAmount = moveSpeed * Time.deltaTime * movementFactor;
                if (!moveXZ) transform.position += new Vector3(0f, movementAmount, 0f);
                if (moveXZ) transform.position += new Vector3(0f, 0f, movementAmount);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ChangeDirectionIfNeeded(other);
        }

        void ChangeDirectionIfNeeded(Collider other)
        {
            bool moveXZ = AppResource.instance.GetCurrentScene().moveXZ;
            if (moveXZ) return;
            if (!GameLayer.Matches(other.gameObject.layer, GameLayer.REDIRECTION_WALL)) return;
            xMovementValue = -(xMovementValue);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            Fn.RunOnce(hasRunTakeDamage, b => hasRunTakeDamage = b, () =>
            {
                AppSfx.Play(AppSfx.instance.powerUpExplode);
                UnityFn.CreateEffect(destroyEffect, position, 1f);
                WeaponType weaponType = WeaponType.GetByName(weaponTypeName);
                PowerUpController powerUp = UnityFn.InstantiateObjectWith<PowerUpController>(AppResource.instance.GetPowerUpPrefab(weaponType), transform.position);
                powerUp.Init(weaponType);
                Destroy(gameObject);
            });
        }

        protected void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
    }
}