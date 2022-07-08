using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.EnemyBullet;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Map;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv5Controller : AbstractDestructibleController
    {
        private GameStoreData storeData;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;
        [SerializeField] private GameObject bossTrigger;
        [SerializeField] private GameObject modPrefab;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int modPositionDelta = 5;
        [SerializeField] private int modDelayToChangeVelocity = 3;
        [SerializeField] private int shotPositionDelta = 4;

        private readonly IntervalState shotInterval = IntervalState.Create(4f);
        private readonly IntervalState modsInterval = IntervalState.Create(5f);

        private Animator animatorCtrl;
        private TriggerByAnyPlayerEnterController bossTriggerCtrl;
        private AppMusic musicController;
        public int hp = 100;

        private int phase = 0;
        private static readonly int openDoorKey = Animator.StringToHash("openDoor");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            bossTriggerCtrl = bossTrigger.GetComponent<TriggerByAnyPlayerEnterController>();
            animatorCtrl = gameObject.GetComponent<Animator>();
            animatorCtrl.enabled = false;
        }

        void Update()
        {
            if (!bossTriggerCtrl.isActivated) return;
            if (phase == 0) HandlePhase0();
            if (phase == 1) HandlePhase1();
            if (phase == 2) HandlePhase2();
        }

        private void HandlePhase0()
        {
            musicController.PlayLv5BossMusic();
            animatorCtrl.enabled = true;
            bossCamera.SetActive(true);
            gameCamera.SetActive(false);
            phase = 1;
        }

        private void HandlePhase1()
        {
            DropMods();
            FireShots();
        }

        void DropMods()
        {
            UnityFn.RunWithInterval(AppResource.instance, modsInterval, () =>
            {
                animatorCtrl.SetTrigger(openDoorKey);
                List<Vector3> deltas = new List<Vector3>() {Vector3.left, Vector3.right};
                SafeSetTimeOut(1f, () =>
                {
                    if (phase != 1) return;
                    Fn.Times(deltas.Count, (i) =>
                    {
                        Vector3 delta = (deltas[i] * modPositionDelta);
                        Vector3 position = transform.position + delta;
                        GameObject mod = Instantiate(modPrefab, position, Quaternion.identity);
                        EnemyWalkerController modCtrl = mod.GetComponent<EnemyWalkerController>();
                        Rigidbody copyRb = mod.GetComponent<Rigidbody>();
                        UnityFn.Throw(copyRb, delta.x * 2, 5f, 0f);
                        UnityFn.SetTimeout(this, modDelayToChangeVelocity, () =>
                        {
                            if (!modCtrl.isBroken) copyRb.velocity /= 4;
                        });
                    });
                });
            });
        }

        void FireShots()
        {
            UnityFn.RunWithInterval(AppResource.instance, shotInterval, () =>
            {
                List<Vector3> deltas = new List<Vector3>() {Vector3.left, Vector3.zero, Vector3.right};
                Fn.Times(deltas.Count, (i) =>
                {
                    Vector3 targetPosition = transform.position + (deltas[i] * shotPositionDelta) + (Vector3.down * shotPositionDelta);
                    Enemy3DFollowerController.Spawn(targetPosition, bulletPrefab);
                });
            });
        }

        public override void TakeDamage(Vector3 position, int damage)
        {
            AppSfx.PlayAdjusted(AppSfx.instance.bigEnemyDamaged);
            hp -= damage;
            if (hp <= 0) phase = 2;
        }

        private void HandlePhase2()
        {
            if (phase == 3) return; // prevent calling this multiple times
            phase = 3; // this is just to prevent getting into here again
            gameCamera.SetActive(true);
            bossCamera.SetActive(false);
            animatorCtrl.enabled = false;
            AppMusic.instance.Stop();
            UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 5f);
            UnityFn.SetTimeout(AppResource.instance, 5, () =>
            {
                AppSfx.instance.levelClear.Play();
                UnityFn.SetTimeout(AppResource.instance, 5, UnityFn.LoadNextScene);
                Destroy(gameObject);
            });
        }

        public override float GetDetectionRange()
        {
            return -1;
        }
    }
}