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
    public class EnemyGrenadeController : MonoBehaviour
    {
        public GameObject impactEffect;

        private Rigidbody rb;
        private EnemyBulletType enemyBulletType;
        private GameObject shotDestination;
        private Vector3 targetPosition;

        public static EnemyGrenadeController Spawn(Vector3 shotPosition, Transform closestPlayerTransform, EnemyBulletType enemyBulletType)
        {
            GameObject prefab = AppResource.instance.GetEnemyBulletPrefab(enemyBulletType);
            EnemyGrenadeController copy = Instantiate(prefab, shotPosition, Quaternion.identity).GetComponent<EnemyGrenadeController>();
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, true);
            copy.enemyBulletType = enemyBulletType;
            copy.targetPosition = closestPlayerTransform.position;
            return copy;
        }

        private void Awake()
        {
            gameObject.layer = GameLayer.ENEMY_SHOT.GetLayer();
        }

        private void Start()
        {
            UnityFn.SetTimeout(this, enemyBulletType.autoDestroyTime, () => Destroy(gameObject));
        }

        void Update()
        {
            UpdateBulletPosition();
        }

        void UpdateBulletPosition()
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