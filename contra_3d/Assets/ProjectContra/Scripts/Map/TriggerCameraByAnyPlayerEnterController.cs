using ProjectContra.Scripts.Types;
using UnityEngine;

namespace ProjectContra.Scripts.Map
{
    public class TriggerCameraByAnyPlayerEnterController : MonoBehaviour
    {
        public bool isActivated = false; // used to be checked to trigger action

        [SerializeField] private GameObject oldCamera;
        [SerializeField] private GameObject newCamera;

        void Start()
        {
            gameObject.layer = GameLayer.INVISIBLE_WALL_TO_PLAYER.GetLayer();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isActivated) return;
            if (GameLayer.Matches(other.gameObject.layer, GameLayer.PLAYER))
            {
                isActivated = true;
                newCamera.SetActive(true);
                oldCamera.SetActive(false);
            }
        }
    }
}