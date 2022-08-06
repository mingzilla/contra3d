using TMPro;
using UnityEngine;

namespace ProjectContra.Scripts.BaseCommon
{
    public class AppVersionRichController : MonoBehaviour
    {
        private void Start()
        {
            TMP_Text text = gameObject.GetComponent<TMP_Text>(); // The TextMeshPro component is TMP_Text
            if (text != null) text.text = $"v {Application.version}";
        }
    }
}