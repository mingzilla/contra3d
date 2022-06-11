using UnityEngine;

namespace ProjectContra.Scripts.GameDataScriptable
{
    [CreateAssetMenu(menuName = "Game State/SceneInitData", fileName = "SceneInitData")]
    public class SceneInitData : ScriptableObject
    {
        public Vector3 playerInitPosition = Vector3.zero;
        public bool moveXZ = false;

        public Vector3 GetRandomPlayerInitPosition()
        {
            float x = Random.Range(playerInitPosition.x, playerInitPosition.x + 2);
            return new Vector3(x, playerInitPosition.y, playerInitPosition.z);
        }
    }
}