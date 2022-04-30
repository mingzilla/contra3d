using System;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.EnemyBullet
{
    public class EnemyBasicBulletController : MonoBehaviour
    {
        public GameObject impactEffect;

        private Rigidbody rb;
        private EnemyBulletType enemyBulletType;
        private GameObject shotDestination;
        private Vector3 targetPosition;

        public static EnemyBasicBulletController Spawn(Vector3 shotPosition, Transform closestPlayerTransform, EnemyBulletType enemyBulletType)
        {
            GameObject prefab = AppResource.instance.GetEnemyBulletPrefab(enemyBulletType);
            EnemyBasicBulletController copy = Instantiate(prefab, shotPosition, Quaternion.identity).GetComponent<EnemyBasicBulletController>();
            copy.gameObject.layer = GameLayer.ENEMY_SHOT.GetLayer();
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, false);
            copy.enemyBulletType = enemyBulletType;
            copy.targetPosition = closestPlayerTransform.position;
            copy.transform.rotation = UnityFn.GetImmediateRotation3D(shotPosition, copy.targetPosition); // set rotation once since bullet goes one direction
            return copy;
        }

        private void Start()
        {
            UnityFn.SetTimeout(this, enemyBulletType.autoDestroyTime, () => Destroy(gameObject));
        }

        void Update()
        {
            MovementUtil.MoveTowardsPosition3D(transform, targetPosition, enemyBulletType.bulletSpeed, delta => targetPosition += delta);
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 position = transform.position;
            UnityFn.CreateEffect(impactEffect, position, 1f); // only if the bullet creates explosion
            Destroy(gameObject);
            GameFn.DealDamage(position, enemyBulletType.blastRange, enemyBulletType.destructibleLayers, (obj) =>
            {
                // EnemyController enemy = obj.GetComponent<EnemyController>();
                // enemy.TakeDamage(position, enemyBulletType.damage);
            });
        }
    }
}