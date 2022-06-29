using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Map;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.EnemyBullet
{
    public class EnemyDropBombController : EnemyBulletController
    {
        private Rigidbody rb;
        [SerializeField] private GameObject dropBombTrigger;
        [SerializeField] private float dropDelay = 0f;
        private TriggerByAnyPlayerEnterController bombTriggerCtrl;
        private bool isTriggered = false;

        private void Start()
        {
            gameObject.layer = GameLayer.ENEMY_GRENADE.GetLayer();
            rb = UnityFn.AddRigidbody(gameObject, false, false);
            bombTriggerCtrl = dropBombTrigger.GetComponent<TriggerByAnyPlayerEnterController>();
        }

        void Update()
        {
            if (!bombTriggerCtrl) return;
            if (!bombTriggerCtrl.isActivated) return;
            if (isTriggered) return;
            isTriggered = true;
            UnityFn.SetTimeout(AppResource.instance, dropDelay, () => Fn.SafeRun(() => rb.useGravity = true));
        }

        private void OnTriggerEnter(Collider other)
        {
            bool hitsPlayer = GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER);
            bool hitsGround = GameLayer.Matches(other.gameObject.layer, GameLayer.THIN_GROUND);
            if (!hitsPlayer && !hitsGround) return;
            DealDamageToPlayer(other, EnemyBulletType.GRENADE);
            AppSfx.Play(AppSfx.instance.grenadeExploded);
            DestroySelf(AppResource.instance.enemyGrenadeSmallExplosion, 1f);
        }
    }
}