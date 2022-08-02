using UnityEngine;

namespace ProjectContra.Scripts.Screens
{
    public class LobbyPlayerPlaceholderController : MonoBehaviour
    {
        [SerializeField] private GameObject joinText;
        [SerializeField] private GameObject readyState;

        public void SetPlayerJoinedStatus()
        {
            joinText.SetActive(false);
        }

        public void SetPlayerLeftStatus()
        {
            joinText.SetActive(true);
        }

        public void SetPlayerReadyTrue()
        {
            readyState.SetActive(true);
        }

        public void SetPlayerReadyFalse()
        {
            readyState.SetActive(false);
        }
    }
}