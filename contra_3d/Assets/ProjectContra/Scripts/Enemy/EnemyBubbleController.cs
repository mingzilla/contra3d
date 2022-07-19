using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBubbleController : AbstractDestructibleController
    {
        [SerializeField] private float delayToChangeVelocity = 1f;

        private Rigidbody rb;
        private Vector3 laterVelocity;
        private int autoDestroyTime;

        public static EnemyBubbleController Spawn(GameObject prefab, Vector3 spawnPosition, Vector3 initialVelocity, int bubbleLifeTime)
        {
            EnemyBubbleController copy = Instantiate(prefab, spawnPosition, Quaternion.identity).GetComponent<EnemyBubbleController>();
            copy.gameObject.layer = GameLayer.ENEMY.GetLayer();
            copy.rb = UnityFn.GetOrAddRigidbody(copy.gameObject, false, false);
            copy.rb.velocity = initialVelocity;
            copy.laterVelocity = initialVelocity / 4;
            copy.autoDestroyTime = bubbleLifeTime;
            return copy;
        }

        private void Start()
        {
            UnityFn.SetTimeout(this, delayToChangeVelocity, () => rb.velocity = laterVelocity);
            UnityFn.SetTimeout(this, autoDestroyTime, () => Destroy(gameObject));
        }

        private void OnCollisionEnter(Collision other)
        {
            EnemyUtil.DealDamageToPlayer(transform.position, 1, other.collider);
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER)) TakeDamage(transform.position, 1);
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            ReduceHpAndCreateEffect(position, damage);
        }

        public override float GetDetectionRange()
        {
            return -1;
        }
    }
}