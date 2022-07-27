using BaseUtil.GameUtil;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectContra.Scripts.Player
{
    public class EndingScreenController : MonoBehaviour
    {
        private static EndingScreenController instance;
        [SerializeField] private GameObject titleText;
        private string title;

        public static EndingScreenController GetInstance()
        {
            if (instance != null) return instance;
            instance = Instantiate(AppResource.instance.infoScreenPrefab, Vector3.zero, Quaternion.identity).GetComponent<EndingScreenController>();
            return instance;
        }

        public void Init()
        {
            titleText.GetComponent<Text>().text = "";
            AppMusic.instance.PlayEndingMusic();
        }

        public void HandleUpdate(UserInput userInput)
        {
            if (userInput.fire1 || userInput.space)
            {
                SceneManager.LoadScene(0);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}