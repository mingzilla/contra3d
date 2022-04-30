using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyShooterController : EnemyController
    {
        public int damage = 1;

        private GameStoreData storeData;
        private Rigidbody rb;
        private CapsuleCollider theCollider;
        private LayerMask destructibleLayers;
        private GameObject destroyEffect;

        private bool canFireShot = true;

        void Start()
        {
            storeData = AppResource.instance.storeData;
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidBodyAndFreezeZ(gameObject);
            destructibleLayers = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});
            destroyEffect = AppResource.instance.smallExplosionPrefab;
        }

        // Update is called once per frame
        void Update()
        {
            if (!storeData.HasPlayer()) return;
            Vector3 position = transform.position;
            Transform closestPlayer = storeData.GetClosestPlayer(position).inGameTransform;
            if (UnityFn.IsInRange(transform, closestPlayer, GetDetectionRange()))
            {
                transform.LookAt(closestPlayer);
                FireShots(position, closestPlayer);
            }
        }

        void FireShots(Vector3 position, Transform closestPlayer)
        {
            if (canFireShot)
            {
                canFireShot = false;
                UnityFn.SetTimeout(this, 3, () =>
                {
                    canFireShot = true;
                    EnemyBasicBulletController.Spawn(position, closestPlayer, EnemyBulletType.BASIC);
                });
            }
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
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER))
            {
                Vector3 location = transform.position;
                GameFn.DealDamage(location, 2, destructibleLayers, (obj) =>
                {
                    CharacterInGameController character = obj.GetComponent<CharacterInGameController>();
                    character.TakeDamage(location, damage);
                });
            }
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            UnityFn.CreateEffect(destroyEffect, position, 1f);
            Destroy(gameObject);
        }

        public override float GetDetectionRange()
        {
            return 50f;
        }
    }
}