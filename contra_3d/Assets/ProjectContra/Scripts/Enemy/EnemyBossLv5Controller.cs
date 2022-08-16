using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AbstractController;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.GameData;
using ProjectContra.Scripts.Util;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv5Controller : AbstractRangeDetectionController
    {
        private GameStoreData storeData;

        [SerializeField] private GameObject gameCamera;
        [SerializeField] private GameObject bossCamera;
        [SerializeField] private GameObject bossBody;
        [SerializeField] private GameObject modPrefab;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int modPositionDelta = 5;
        [SerializeField] private int modDelayToChangeVelocity = 3;
        [SerializeField] private int shotPositionDelta = 4;
        [SerializeField] private int bulletMoveSpeed = 7;
        [SerializeField] private GameObject weakPoint;
        private EnemyBossWeakPointController weakPointController;

        private readonly IntervalState shotInterval = IntervalState.Create(4f);

        private Animator animatorCtrl;
        private Animator bodyAnimatorCtrl;
        private AppMusic musicController;

        private int phase = 0;
        private static readonly int openDoorKey = Animator.StringToHash("openDoor");

        void Start()
        {
            storeData = AppResource.instance.storeData;
            musicController = AppResource.instance.musicManager.GetComponent<AppMusic>();
            animatorCtrl = gameObject.GetComponent<Animator>();
            animatorCtrl.enabled = false;
            bodyAnimatorCtrl = bossBody.GetComponent<Animator>();
            bodyAnimatorCtrl.enabled = false;
            weakPointController = weakPoint.GetComponent<EnemyBossWeakPointController>();
            UnityFn.FastSetActive(weakPoint, false);
        }

        void Update()
        {
            if (phase == 0)
                RunIfPlayerIsInRange(storeData, GetDetectionRange(), (closestPlayer) =>
                {
                    HandlePhase0();
                });
            if (phase == 1) HandlePhase1();
            if (phase == 2) HandlePhase2();
        }

        private void HandlePhase0()
        {
            musicController.PlayLv5BossMusic();
            animatorCtrl.enabled = true;
            bodyAnimatorCtrl.enabled = true;
            bossCamera.SetActive(true);
            gameCamera.SetActive(false);
            UnityFn.FastSetActive(weakPoint, true);
            phase = 1;
        }

        private void HandlePhase1()
        {
            UnityFn.RunWithInterval(this, shotInterval, () =>
            {
                animatorCtrl.enabled = false;
                UnityFn.SetTimeout(this, 1.5f, () => animatorCtrl.enabled = true);
                DropMods();
                FireShots();
            });
            if (weakPointController.isBroken) phase = 2;
        }

        void DropMods()
        {
            bodyAnimatorCtrl.SetTrigger(openDoorKey);
            List<Vector3> deltas = new List<Vector3>() {Vector3.left, Vector3.right};
            UnityFn.SetTimeout(this, 0.5f, () =>
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
        }

        void FireShots()
        {
            List<Vector3> deltas = new List<Vector3>() {Vector3.left, Vector3.zero, Vector3.right};
            Fn.Times(deltas.Count, (i) =>
            {
                Vector3 targetPosition = transform.position + (deltas[i] * shotPositionDelta) + (Vector3.down * shotPositionDelta);
                Enemy3DFollowerController bullet = Enemy3DFollowerController.Spawn(targetPosition, bulletPrefab);
                bullet.moveSpeed = 0;
                UnityFn.SetTimeout(this, 1f, () =>
                {
                    if (!bullet.isBroken) bullet.moveSpeed = bulletMoveSpeed;
                });
            });
        }

        private void HandlePhase2()
        {
            phase = 3; // this is just to prevent getting into here again
            StopAllCoroutines();
            shotInterval.Reset();
            gameCamera.SetActive(true);
            bossCamera.SetActive(false);
            animatorCtrl.enabled = false;
            bodyAnimatorCtrl.enabled = false;
            AppMusic.instance.Stop();
            AbstractDestructibleController.KillAllByType<EnemyWalkerController>();
            AbstractDestructibleController.KillAllByType<Enemy3DFollowerController>();
            UnityFn.CreateEffect(AppResource.instance.enemyDestroyedBigExplosion, transform.position, 5f);
            GameFn.LoadNextSceneAfterBossKilled(gameObject, false);
        }

        public override float GetDetectionRange()
        {
            return 60;
        }
    }
}