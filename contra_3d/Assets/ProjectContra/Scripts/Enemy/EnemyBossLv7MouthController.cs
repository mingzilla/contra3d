using System;
using System.Collections.Generic;
using BaseUtil.Base;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public class EnemyBossLv7MouthController : MonoBehaviour
    {
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private int shotPositionDelta = 4;
        [SerializeField] private int bulletMoveSpeed = 7;
        [SerializeField] private int shotInterval = 4;

        private IntervalState shotIntervalState;

        private void Start()
        {
            shotIntervalState = IntervalState.Create(shotInterval);
        }

        void Update()
        {
            UnityFn.RunWithInterval(this, shotIntervalState, () =>
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
            });
        }
    }
}