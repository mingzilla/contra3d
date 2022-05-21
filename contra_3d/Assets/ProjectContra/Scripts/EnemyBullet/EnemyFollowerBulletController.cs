using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.EnemyBullet
{
    /// <summary>
    /// This type of bullet constantly follows the player, with instant direction changes, so it's slow and destroyable.
    /// </summary>
    public class EnemyFollowerBulletController : EnemyBulletController
    {
        private Rigidbody rb;
        private GameObject impactEffect;
        private EnemyBulletType enemyBulletType;
        private Transform closestPlayerTransform; // reference to the player, which changes if player moves 

        public static EnemyFollowerBulletController Spawn(Vector3 shotPosition, Transform closestPlayerTransform, EnemyBulletType enemyBulletType)
        {
            GameObject prefab = AppResource.instance.GetEnemyBulletPrefab(enemyBulletType);
            EnemyFollowerBulletController copy = Instantiate(prefab, shotPosition, Quaternion.identity).GetComponent<EnemyFollowerBulletController>();
            copy.gameObject.layer = GameLayer.ENEMY_DESTROYABLE_SHOT.GetLayer();
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, false, 1f);
            copy.impactEffect = AppResource.instance.enemyBulletHitEffect;
            copy.enemyBulletType = enemyBulletType;
            copy.closestPlayerTransform = closestPlayerTransform;
            return copy;
        }

        private void Start()
        {
            UnityFn.SetTimeout(this, enemyBulletType.autoDestroyTime, () => Destroy(gameObject));
        }

        void Update()
        {
            MovementUtil.FollowTowardsPosition3D(transform, closestPlayerTransform, 0f, enemyBulletType.bulletSpeed, Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            DealDamageToPlayer(other, enemyBulletType);
            DestroySelf(impactEffect, 1f);
        }
    }
}