using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class Enemy3DFollowerController : AbstractDestructibleController
    {
        [SerializeField] private float detectionRange = 40f;
        [SerializeField] public float moveSpeed = 10f;
        [SerializeField] private bool enableRotation = true;
        [SerializeField] private bool freezeZ = false;
        private GameStoreData storeData;

        private Rigidbody rb;

        public static Enemy3DFollowerController Spawn(Vector3 spawnPosition, GameObject prefab)
        {
            Enemy3DFollowerController copy = Instantiate(prefab, spawnPosition, Quaternion.identity).GetComponent<Enemy3DFollowerController>();
            return copy;
        }

        private void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.GetOrAddRigidbody(gameObject, false, freezeZ);
        }

        void Update()
        {
            RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                float deltaTime = Time.deltaTime;
                if (enableRotation) transform.Rotate(0, 0, 360 * deltaTime);
                transform.position = UnityFn.GetPosition(transform, closestPlayer, moveSpeed, deltaTime);
            });
        }

        private void OnCollisionEnter(Collision other)
        {
            EnemyUtil.DealDamageToPlayer(transform.position, 1, other.collider);
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER)) TakeDamage(transform.position, 1);
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(AppResource.instance.enemyBulletHitEffect, position, 1f);
            AppSfx.PlayAdjusted(AppSfx.instance.enemyDeath);
            Destroy(gameObject);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}