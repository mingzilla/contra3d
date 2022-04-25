using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyWalkerController : EnemyController
    {
        public int damage = 1;

        private Rigidbody rb;
        private CapsuleCollider theCollider;
        private LayerMask destructibleLayers;
        private GameObject destroyEffect;


        void Start()
        {
            gameObject.layer = GameLayer.ENEMY.GetLayer();
            rb = UnityFn.AddRigidBodyAndFreezeZ(gameObject);
            destructibleLayers = GameLayer.GetLayerMask(new List<GameLayer>() {GameLayer.PLAYER});
            destroyEffect = AppResource.instance.smallExplosionPrefab;
        }

        // Update is called once per frame
        void Update()
        {
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
            Debug.Log("Enemy took " + damage);
            UnityFn.CreateEffect(destroyEffect, position, 1f);
        }
    }
}