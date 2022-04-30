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
        private float xForce = 2f;
        private float yForce = 8f;
        private float zForce = 0f;

        public static EnemyGrenadeController Spawn(Vector3 shotPosition, float xForce, float yForce, float zForce, EnemyBulletType enemyBulletType)
        {
            GameObject prefab = AppResource.instance.GetEnemyBulletPrefab(enemyBulletType);
            EnemyGrenadeController copy = Instantiate(prefab, shotPosition, Quaternion.identity).GetComponent<EnemyGrenadeController>();
            copy.gameObject.layer = GameLayer.ENEMY_GRENADE.GetLayer();
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, true);
            copy.enemyBulletType = enemyBulletType;
            copy.xForce = xForce;
            copy.yForce = yForce;
            copy.zForce = zForce;
            return copy;
        }

        private void Start()
        {
            UnityFn.SetTimeout(this, enemyBulletType.autoDestroyTime, () => Destroy(gameObject));
            UnityFn.Throw(rb, xForce, yForce, zForce);
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