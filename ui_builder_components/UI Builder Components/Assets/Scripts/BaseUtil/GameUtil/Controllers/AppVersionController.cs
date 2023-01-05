using UnityEngine;
using UnityEngine.UI;

namespace BaseUtil.GameUtil.Controllers
{
    public abstract class AppVersionController : MonoBehaviour
    {
        [SerializeField] private Text text;

        private void Start()
        {
            text = GetComponent<Text>();
            if (text != null) text.text = $"v {Application.version}";
        }
    }
}