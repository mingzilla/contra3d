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
    /// <summary>
    /// This type of bullet constantly follows the player, with instant direction changes, so it's slow and destroyable.
    /// </summary>
    public class EnemyFollowerBulletController : MonoBehaviour
    {
        public GameObject impactEffect;

        private Rigidbody rb;
        private EnemyBulletType enemyBulletType;
        private Transform closestPlayerTransform; // reference to the player, which changes if player moves 

        public static EnemyFollowerBulletController Spawn(Vector3 shotPosition, Transform closestPlayerTransform, EnemyBulletType enemyBulletType)
        {
            GameObject prefab = AppResource.instance.GetEnemyBulletPrefab(enemyBulletType);
            EnemyFollowerBulletController copy = Instantiate(prefab, shotPosition, Quaternion.identity).GetComponent<EnemyFollowerBulletController>();
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject);
            copy.enemyBulletType = enemyBulletType;
            copy.closestPlayerTransform = closestPlayerTransform;
            return copy;
        }

        private void Awake()
        {
            gameObject.layer = GameLayer.ENEMY_DESTROYABLE_SHOT.GetLayer();
        }

        private void Start()
        {
            UnityFn.SetTimeout(this, enemyBulletType.autoDestroyTime, () => Destroy(gameObject));
        }

        void Update()
        {
            MovementUtil.MoveTowardsPosition3D(transform, closestPlayerTransform, 0f, enemyBulletType.bulletSpeed, Time.deltaTime);
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