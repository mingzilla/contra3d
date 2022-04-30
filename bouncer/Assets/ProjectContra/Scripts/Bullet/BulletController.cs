using System;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Bullet
{
    public class BulletController : MonoBehaviour
    {
        public GameObject impactEffect;

        private Rigidbody rb;
        private Vector3 moveDirection;
        private WeaponType weaponType;

        public static BulletController Spawn(Transform shotPoint, bool isFacingForward, UserInput userInput, WeaponType weaponType, bool isOnGround)
        {
            GameObject prefab = AppResource.instance.GetBulletPrefab(weaponType);
            BulletController copy = Instantiate(prefab, shotPoint.position, shotPoint.rotation).GetComponent<BulletController>();
            copy.moveDirection = BulletCommonUtil3D.CreateBulletDirection(isFacingForward, userInput, isOnGround);
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, false);
            copy.weaponType = weaponType;
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
            UnityFn.CreateEffect(impactEffect, position, 1f); // only if the bullet creates explosion
            Destroy(gameObject);
            GameFn.DealDamage(position, weaponType.blastRange, weaponType.destructibleLayers, (obj) =>
            {
                EnemyController enemy = obj.GetComponent<EnemyController>();
                enemy.TakeDamage(position, weaponType.damage);
            });
        }
    }
}