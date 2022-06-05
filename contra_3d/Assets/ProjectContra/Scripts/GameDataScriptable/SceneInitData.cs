using UnityEngine;

namespace ProjectContra.Scripts.GameDataScriptable
{
    [CreateAssetMenu(menuName = "Game State/SceneInitData", fileName = "SceneInitData")]
    public class SceneInitData : ScriptableObject
    {
        public Vector3 playerInitPosition = Vector3.zero;
    }
}