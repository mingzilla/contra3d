using BaseUtil.Base;
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
            rb = UnityFn.AddRigidbody(gameObject, false, true);
            UnityFn.AddSphereCollider(gameObject, 1f, false);
            meshRenderer = UnityFn.MakeInvisible<MeshRenderer>(gameObject);
            destroyEffect = AppResource.instance.powerUpDestroyedSmallExplosion;
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
                float movementFactor = MovementUtil.CalculateCircularMovementFactor(1f);
                transform.position += new Vector3(0f, (moveSpeed * Time.deltaTime * movementFactor), 0f);
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            Fn.RunOnce(hasRunTakeDamage, b => hasRunTakeDamage = b, () =>
            {
                UnityFn.CreateEffect(destroyEffect, position, 1f);
                WeaponType weaponType = WeaponType.GetByName(weaponTypeName);
                PowerUpController powerUp = UnityFn.InstantiateObjectWith<PowerUpController>(AppResource.instance.GetPowerUpPrefab(weaponType), transform.position);
                powerUp.Init(weaponType);
                Destroy(gameObject);
            });
        }
    }
}