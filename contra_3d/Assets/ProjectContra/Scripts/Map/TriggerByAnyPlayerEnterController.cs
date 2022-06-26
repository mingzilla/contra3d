using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class TriggerByAnyPlayerEnterController : MonoBehaviour
    {
        public bool isActivated = false; // used to be checked to trigger action

        void Start()
        {
            gameObject.layer = GameLayer.INVISIBLE_WALL_TO_PLAYER.GetLayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isActivated) return;
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER)) isActivated = true;
        }
    }
}