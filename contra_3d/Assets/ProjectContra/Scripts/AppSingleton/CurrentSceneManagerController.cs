using ProjectContra.Scripts.Util;
using UnityEngine;

namespace ProjectContra.Scripts.AppSingleton
{
    /// <summary>
    /// Manages the current scene. Need instantiation per scene so that the relevant content is loaded.
    /// Don't put this as part of GameManager, this object needs to be destroyable
    /// </summary>
    public class CurrentSceneManagerController : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneUtil.InitializeScene();
        }
    }
}