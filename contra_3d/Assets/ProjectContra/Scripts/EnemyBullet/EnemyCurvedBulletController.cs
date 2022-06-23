using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.EnemyBullet
{
    public class EnemyCurvedBulletController : EnemyBulletController
    {
        private Rigidbody rb;
        private GameObject impactEffect;
        private EnemyBulletType enemyBulletType;
        private GameObject shotDestination;
        private Vector3 targetPosition;
        private bool moveLeft = true;

        public static EnemyCurvedBulletController Spawn(Vector3 shotPosition, EnemyBulletType enemyBulletType, bool moveLeft)
        {
            GameObject prefab = AppResource.instance.GetEnemyBulletPrefab(enemyBulletType);
            EnemyCurvedBulletController copy = Instantiate(prefab, shotPosition, Quaternion.identity).GetComponent<EnemyCurvedBulletController>();
            copy.gameObject.layer = GameLayer.ENEMY_SHOT.GetLayer();
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, false, 1f);
            copy.impactEffect = AppResource.instance.enemyBulletHitEffect;
            copy.enemyBulletType = enemyBulletType;
            copy.targetPosition = shotPosition + new Vector3(-1f, 0f, 0f);
            copy.moveLeft = moveLeft;
            copy.transform.rotation = UnityFn.GetImmediateRotation3D(shotPosition, copy.targetPosition); // set rotation once since bullet goes one direction
            return copy;
        }

        private void Start()
        {
            UnityFn.SetTimeout(this, enemyBulletType.autoDestroyTime, () => Destroy(gameObject));
        }

        void Update()
        {
            int xMovementValue = moveLeft ? -1 : 1;
            MovementUtil.MoveX(transform, xMovementValue, enemyBulletType.bulletSpeed);
            float movementFactor = MovementUtil.CalculateCircularMovementFactor(1f);
            float movementAmount = enemyBulletType.bulletSpeed * Time.deltaTime * movementFactor;
            transform.position += new Vector3(0f, movementAmount, 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            DealDamageToPlayer(other, enemyBulletType);
            if (enemyBulletType.destroyWhenHit) DestroySelf(impactEffect, 1f);
        }
    }
}