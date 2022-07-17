using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Enemy.Util;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyWalkerController : AbstractDestructibleController
    {
        public int damage = 1;
        public int moveSpeed = 8;
        public float jumpForce = 10f;
        public float detectionRange = 60f;
        private bool isActive = false;
        [SerializeField] private bool forceFollowingPlayer = false;
        [SerializeField] private int selfDestroyTime = -1; // if this is higher than 0, it can self destroy

        private GameStoreData storeData;
        private Rigidbody rb;
        private CapsuleCollider theCollider;
        private bool moveXLeft = false;
        private Animator animatorCtrl;
        private static readonly int isRunning = Animator.StringToHash("isRunning");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            bool moveXZ = AppResource.instance.GetCurrentScene().moveXZ;
            rb = UnityFn.GetOrAddRigidbody(gameObject, true, !moveXZ);
            animatorCtrl = gameObject.GetComponent<Animator>();
            if (selfDestroyTime > 0)
                UnityFn.SetTimeout(this, selfDestroyTime, () =>
                {
                    TakeDamage(transform.position, maxHp);
                });
        }

        void Update()
        {
            if (!storeData.HasPlayer()) return;
            Move();
        }

        void Move()
        {
            Transform closestPlayer = storeData.GetClosestPlayer(transform.position).inGameTransform;
            if (!isActive && UnityFn.IsInRange(transform, closestPlayer, GetDetectionRange()))
            {
                isActive = true;
                moveXLeft = closestPlayer.position.x < transform.position.x;
                if (animatorCtrl) animatorCtrl.SetBool(isRunning, true);
                transform.rotation = UnityFn.LookXZ(transform, closestPlayer);
            }
            bool moveXZ = AppResource.instance.GetCurrentScene().moveXZ;
            bool followPlayer = forceFollowingPlayer || moveXZ;
            if (isActive && !followPlayer) MovementUtil.MoveX(transform, (moveXLeft ? -1 : 1), moveSpeed);
            if (isActive && followPlayer) MovementUtil.FollowXZTowardsPosition3D(transform, closestPlayer, 0.5f, moveSpeed, Time.deltaTime);
        }

        void OnCollisionEnter(Collision other)
        {
            DealDamage(other);
        }

        void OnCollisionStay(Collision other)
        {
            DealDamage(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            JumpAtTriggerPoint(other);
            ChangeDirectionIfNeeded(other);
        }

        void ChangeDirectionIfNeeded(Collider other)
        {
            bool moveXZ = AppResource.instance.GetCurrentScene().moveXZ;
            if (moveXZ) return;
            if (!GameLayer.Matches(other.gameObject.layer, GameLayer.REDIRECTION_WALL)) return;
            rb.velocity = Vector3.zero; // so that direction change is not affected by the original inertia
            moveXLeft = !moveXLeft;
            transform.rotation = UnityFn.LookX(transform, !moveXLeft);
        }

        void DealDamage(Collision other)
        {
            EnemyUtil.DealDamageToPlayer(transform.position, damage, other.collider);
        }

        private void JumpAtTriggerPoint(Collider other)
        {
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.ENEMY_JUMP_POINT))
            {
                GameFn.HandleJump(rb, jumpForce);
            }
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            ReduceHpAndCreateEffect(position, damage);
        }

        public override float GetDetectionRange()
        {
            return detectionRange;
        }

        protected void OnBecameInvisible()
        {
            isBroken = true;
            Destroy(gameObject);
        }
    }
}