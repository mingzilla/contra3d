using ProjectContra.Scripts.AppSingleton.LiveResource;
using UnityEngine;

namespace ProjectContra.Scripts.Screens
{
    public class ButtonEventController : MonoBehaviour
    {
        public void OnMove()
        {
            AppSfx.PlayAdjusted(AppSfx.instance.menuSelect);
        }
    }
}