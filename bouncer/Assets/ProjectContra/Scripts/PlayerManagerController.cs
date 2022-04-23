using UnityEngine;
using ProjectContra.Scripts.Types;

namespace ProjectContra.Scripts
{
    public class PlayerManagerController : MonoBehaviour
    {
        private void Awake()
        {
            GameTag.InitOnAwake();
        }
    }
}