using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Bullet
{
    public class BulletController : MonoBehaviour
    {
        private Rigidbody rb;
        private GameObject explosionEffect;
        private Vector3 moveDirection;
        private WeaponType weaponType;

        public static BulletController Spawn(Transform shotPoint, Vector3 positionDelta, bool isFacingForward, UserInput userInput, WeaponType weaponType, bool isOnGround)
        {
            GameObject prefab = AppResource.instance.GetBulletPrefab(weaponType);
            BulletController copy = Instantiate(prefab, (shotPoint.position + positionDelta), shotPoint.rotation).GetComponent<BulletController>();
            copy.moveDirection = BulletCommonUtil3D.CreateBulletDirection(isFacingForward, userInput, isOnGround);
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, false, 1f);
            copy.weaponType = weaponType;
            copy.explosionEffect = AppResource.instance.GetBulletHitEffect(weaponType);
            return copy;
        }

        private void Awake()
        {
            gameObject.layer = GameLayer.PLAYER_SHOT.GetLayer();
        }

        private void Start()
        {
            UnityFn.SetTimeout(this, weaponType.autoDestroyTime, () => Destroy(gameObject));
        }

        void FixedUpdate()
        {
            BulletCommonUtil3D.HandleBulletFixedMovement(rb, moveDirection, weaponType.bulletSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 position = transform.position;
            UnityFn.CreateEffect(explosionEffect, position, 1f); // only if the bullet creates explosion
            Destroy(gameObject);
            GameFn.DealDamage(position, weaponType.blastRange, weaponType.GetDestructibleLayers(), (obj) =>
            {
                AbstractDestructibleController destructible = obj.GetComponent<AbstractDestructibleController>();
                if (destructible != null) destructible.TakeDamage(position, weaponType.damage); // null check to avoid child objects
            });
        }
    }
}