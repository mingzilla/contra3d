using System;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Bullet
{
    public class BulletController : MonoBehaviour
    {
        public float bulletSpeed = 20f;
        public GameObject impactEffect;
        public float autoDestroyTime = 2f;

        private Rigidbody rb;
        private Vector2 moveDirection;
        private WeaponType weaponType;

        private void Awake()
        {
            rb = BulletCommonUtil3D.AddRigidbodyToBullet(gameObject);
            UnityFn.WaitForSeconds(this, autoDestroyTime, () => Destroy(gameObject));
        }

        void Update()
        {
            BulletCommonUtil3D.HandleBulletMovement(rb, moveDirection, bulletSpeed);
        }

        public static BulletController Spawn(Transform shotPoint, bool isFacingForward, UserInput userInput, WeaponType weaponType, bool isOnGround)
        {
            GameObject prefab = AppResource.instance.GetBulletPrefab(weaponType);
            BulletController copy = Instantiate(prefab, shotPoint.position, shotPoint.rotation).GetComponent<BulletController>();
            copy.moveDirection = BulletCommonUtil3D.CreateBulletDirection(isFacingForward, userInput, isOnGround);
            copy.weaponType = weaponType;
            return copy;
        }

        private void OnTriggerEnter(Collider other)
        {
            var position = transform.position;
            UnityFn.CreateEffect(impactEffect, position);
            Destroy(gameObject);
            GameFn.DealDamageToUnit(position, weaponType.blastRange, weaponType.destructibleLayers, weaponType.damage, (stat) => { });
        }
    }
}