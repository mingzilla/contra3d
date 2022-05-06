using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton.LiveResource
{
    public class ResourceTitleScene : MonoBehaviour
    {
        public static ResourceTitleScene instance;

        public GameObject[] lobbyCharacterPanels;

        private void Awake()
        {
            if (!instance) instance = this;
        }
    }
}