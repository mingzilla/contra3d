using System.Collections;
using System.Collections.Generic;
using BaseUtil.GameUtil.Base;
using ProjectContra.Scripts.Player;
using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Enemy
{
    public abstract class EnemyController : MonoBehaviour
    {
        public abstract void TakeDamage(Vector3 position, int damage);
    }
}