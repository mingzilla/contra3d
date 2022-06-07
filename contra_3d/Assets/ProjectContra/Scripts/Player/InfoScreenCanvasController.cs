using BaseUtil.GameUtil;
using ProjectContra.Scripts.AppSingleton;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectContra.Scripts.Player
{
    /// <summary>
    /// A canvas, controlled by a player.
    /// </summary>
    public class InfoScreenCanvasController : MonoBehaviour
    {
        private static InfoScreenCanvasController instance;
        [SerializeField] private GameObject titleText;
        private string title;

        private MusicController musicController;

        public static InfoScreenCanvasController GetInstance()
        {
            if (instance != null) return instance;
            instance = Instantiate(AppResource.instance.infoScreenPrefab, Vector3.zero, Quaternion.identity).GetComponent<InfoScreenCanvasController>();
            return instance;
        }

        public void Init(string text)
        {
            titleText.GetComponent<Text>().text = text;
            musicController = AppResource.instance.musicManager.GetComponent<MusicController>();
        }

        public void HandleUpdate(UserInput userInput)
        {
            if (userInput.fire1 || userInput.space)
            {
                AppResource.instance.storeData.controlState = GameControlState.IN_GAME;
                musicController.PlayByScene(AppResource.instance.storeData.currentScene);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}