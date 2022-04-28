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

        public static EnemyBasicBulletController Spawn(Vector3 shotPosition, Transform closestPlayerTransform, EnemyBulletType enemyBulletType)
        {
            GameObject prefab = AppResource.instance.GetEnemyBulletPrefab(enemyBulletType);
            EnemyBasicBulletController copy = Instantiate(prefab, shotPosition, Quaternion.identity).GetComponent<EnemyBasicBulletController>();
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject);
            copy.enemyBulletType = enemyBulletType;
            UnityFn.LookAtPlayer(copy.transform, closestPlayerTransform);
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
            rb.velocity = transform.position * enemyBulletType.bulletSpeed * Time.deltaTime;
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