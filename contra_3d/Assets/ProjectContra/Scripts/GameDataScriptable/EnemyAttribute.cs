using UnityEngine;

namespace ProjectContra.Scripts.GameDataScriptable
{
    [CreateAssetMenu(menuName = "Game State/EnemyAttribute", fileName = "EnemyAttribute")]
    public class EnemyAttribute: ScriptableObject
    {
        public int hp = 10;
    }
}