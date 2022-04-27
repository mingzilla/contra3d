using BaseUtil.GameUtil.Base;
using UnityEngine;
using ProjectContra.Scripts.Types;

namespace ProjectContra.Scripts.AppSingleton
{
    public class PlayerManagerController : MonoBehaviour
    {
        public static PlayerManagerController instance;
        private void Awake()
        {
            UnityFn.MarkSingletonAndKeepAlive(instance, gameObject, () => instance = this);
            GameTag.InitOnAwake();
            GameLayer.InitOnAwake();
        }
    }
}