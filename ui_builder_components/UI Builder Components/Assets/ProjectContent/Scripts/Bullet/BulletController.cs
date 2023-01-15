using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Util3D;
using ProjectContent.Scripts.Types;
using UnityEngine;

namespace ProjectContent.Scripts.Bullet
{
    public class BulletController : MonoBehaviour
    {
        private Rigidbody rb;
        private GameObject explosionEffect;
        private Vector3 moveDirection;
        public Skill skill;

        public static BulletController SpawnXZ(GameObject prefab, Transform shotPoint, Vector3 positionDelta, Skill skill)
        {
            Vector3 direction = BulletCommonUtil3D.CreateBulletXZDirection(shotPoint.rotation.eulerAngles.y);
            return SpawnInDirection(prefab, shotPoint, positionDelta, skill, direction);
        }

        public static BulletController SpawnInDirection(GameObject prefab, Transform shotPoint, Vector3 positionDelta, Skill skill, Vector3 direction)
        {
            // AppSfx.PlayBulletSound(skill);
            Vector3 shotPointPosition = shotPoint.position + positionDelta;
            BulletController copy = Instantiate(prefab, shotPointPosition, shotPoint.rotation).GetComponent<BulletController>();
            copy.moveDirection = direction;
            copy.rb = BulletCommonUtil3D.AddRigidbodyAndColliderToBullet(copy.gameObject, false, 1f);
            copy.skill = skill;
            copy.transform.rotation = UnityFn.GetImmediateRotation3D(shotPointPosition, (shotPointPosition + direction));
            // copy.explosionEffect = AppResource.instance.GetBulletHitEffect(skill);
            return copy;
        }

        private void Awake()
        {
            gameObject.layer = GameLayer.PLAYER_SHOT.GetLayer();
        }

        private void Start()
        {
            Destroy(gameObject, skill.autoDestroyTime);
        }

        void FixedUpdate()
        {
            BulletCommonUtil3D.HandleBulletFixedMovement(rb, moveDirection, skill.moveSpeed);
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 position = transform.position;
            UnityFn.CreateEffect(explosionEffect, position, 1f); // only if the bullet creates explosion
            // if (explosionEffect != null) AppSfx.PlayAdjusted(AppSfx.instance.bulletFExplode);
            if (skill.destroyWhenHit) Destroy(gameObject);
            // UnityFn.DealDamage(position, skill.blastRange, skill.GetDestructibleLayers(), (obj) =>
            // {
            //     AbstractDestructibleController destructible = obj.GetComponent<AbstractDestructibleController>();
            //     if (destructible != null) destructible.TakeDamage(position, skill.damage); // null check to avoid child objects
            // });
        }
    }
}