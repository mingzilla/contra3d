using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyDestructibleHazardController : AbstractDestructibleController
    {
        private GameStoreData storeData;

        public int hp = 5;
        [SerializeField] private float detectionRange = 40f;
        [SerializeField] private int damage = 1;
        [SerializeField] private bool useGravity = true;
        [SerializeField] private string gameLayer = GameLayer.ENEMY.name;
        private Rigidbody rb;

        private void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.GetByName(gameLayer).GetLayer();
            rb = gameObject.GetComponent<Rigidbody>();
            if (useGravity) rb.useGravity = isTriggered;
        }

        void Update()
        {
            TriggerIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
            {
                if (useGravity) rb.useGravity = true;
            });
        }

        void OnCollisionEnter(Collision other)
        {
            DealDamage(other);
        }

        void OnCollisionStay(Collision other)
        {
            DealDamage(other);
        }

        void DealDamage(Collision other)
        {
            EnemyUtil.DealDamageToPlayer(transform.position, damage, other.collider);
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            AppSfx.Play(AppSfx.instance.bigEnemyDamaged);
            hp -= damage;
            if (hp <= 0)
            {
                UnityFn.CreateEffect(AppResource.instance.enemyDestroyedSmallExplosion, position, 1f);
                AppSfx.Play(AppSfx.instance.grenadeExploded);
                Destroy(gameObject);
            }
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }
    }
}