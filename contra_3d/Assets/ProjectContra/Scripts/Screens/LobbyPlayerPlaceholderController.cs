using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectContra.Scripts.Screens
{
    public class LobbyPlayerPlaceholderController : MonoBehaviour
    {
        [SerializeField] private GameObject joinText;

        public void SetPlayerJoinedStatus()
        {
            joinText.SetActive(false);
        }

        public void SetPlayerLeftStatus()
        {
            joinText.SetActive(true);
        }
    }
}