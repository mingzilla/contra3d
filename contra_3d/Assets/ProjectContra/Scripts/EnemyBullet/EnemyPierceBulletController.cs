using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.EnemyBullet
{
    public class EnemyPierceBulletController : EnemyBulletController
    {
        private Rigidbody rb;
        private EnemyBulletType enemyBulletType;
        private GameObject shotDestination;
        private Vector3 targetPosition;
        private static int xMovementValue = -1; // only fire to the left side

        public static EnemyPierceBulletController Spawn(Vector3 shotPosition, Vector3 closestPlayerPosition, EnemyBulletType enemyBulletType)
        {
            GameObject prefab = AppResource.instance.GetEnemyBulletPrefab(enemyBulletType);
            EnemyPierceBulletController copy = Instantiate(prefab, shotPosition, Quaternion.identity).GetComponent<EnemyPierceBulletController>();
            copy.gameObject.layer = GameLayer.ENEMY_SHOT.GetLayer();
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, false, 1f);
            copy.enemyBulletType = enemyBulletType;
            copy.transform.rotation = UnityFn.GetImmediateRotation3D(shotPosition, (shotPosition + new Vector3(xMovementValue, 0f, 0f)));
            return copy;
        }

        private void Start()
        {
            UnityFn.SetTimeout(this, enemyBulletType.autoDestroyTime, () => Destroy(gameObject));
        }

        void Update()
        {
            MovementUtil.MoveX(transform, xMovementValue, enemyBulletType.bulletSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            DealDamageToPlayer(other, enemyBulletType);
        }
    }
}