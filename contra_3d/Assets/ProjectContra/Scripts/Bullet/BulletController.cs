﻿using BaseUtil.GameUtil;
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
        public WeaponType weaponType;

        public static BulletController Spawn(Transform shotPoint, Vector3 positionDelta, bool isFacingForward, UserInput userInput, WeaponType weaponType, bool isOnGround)
        {
            Vector3 direction = BulletCommonUtil3D.CreateBulletDirection(isFacingForward, userInput, isOnGround);
            return SpawnInDirection(shotPoint, positionDelta, weaponType, direction);
        }

        public static BulletController SpawnXZ(Transform shotPoint, Vector3 positionDelta, WeaponType weaponType)
        {
            Vector3 direction = BulletCommonUtil3D.CreateBulletXZDirection(shotPoint.rotation.eulerAngles.y);
            return SpawnInDirection(shotPoint, positionDelta, weaponType, direction);
        }

        public static BulletController SpawnInDirection(Transform shotPoint, Vector3 positionDelta, WeaponType weaponType, Vector3 direction)
        {
            GameObject prefab = AppResource.instance.GetBulletPrefab(weaponType);
            AppSfx.PlayBulletSound(weaponType);
            Vector3 shotPointPosition = shotPoint.position + positionDelta;
            BulletController copy = Instantiate(prefab, shotPointPosition, shotPoint.rotation).GetComponent<BulletController>();
            copy.moveDirection = direction;
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, false, 1f);
            copy.weaponType = weaponType;
            copy.transform.rotation = UnityFn.GetImmediateRotation3D(shotPointPosition, (shotPointPosition + direction));
            copy.explosionEffect = AppResource.instance.GetBulletHitEffect(weaponType);
            return copy;
        }

        private void Awake()
        {
            gameObject.layer = GameLayer.PLAYER_SHOT.GetLayer();
        }

        private void Start()
        {
            Destroy(gameObject, weaponType.autoDestroyTime);
        }

        void FixedUpdate()
        {
            BulletCommonUtil3D.HandleBulletFixedMovement(rb, moveDirection, weaponType.bulletSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 position = transform.position;
            UnityFn.CreateEffect(explosionEffect, position, 1f); // only if the bullet creates explosion
            if (explosionEffect != null) AppSfx.PlayAdjusted(AppSfx.instance.bulletFExplode);
            if (weaponType.destroyWhenHit) Destroy(gameObject);
            UnityFn.DealDamage(position, weaponType.blastRange, weaponType.GetDestructibleLayers(), (obj) =>
            {
                AbstractDestructibleController destructible = obj.GetComponent<AbstractDestructibleController>();
                if (destructible != null) destructible.TakeDamage(position, weaponType.damage); // null check to avoid child objects
            });
        }
    }
}