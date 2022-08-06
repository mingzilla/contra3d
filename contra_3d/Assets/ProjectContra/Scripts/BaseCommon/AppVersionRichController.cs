using TMPro;
using UnityEngine;

namespace ProjectContra.Scripts.BaseCommon
{
    public class AppVersionRichController : MonoBehaviour
    {
        private void Start()
        {
            TextMeshProUGUI text = gameObject.GetComponent<TextMeshProUGUI>();
            if (text != null) text.text = $"v {Application.version}";
        }
    }
}