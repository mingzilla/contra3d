using System;
using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace ProjectContra.Scripts.Player
{
    public class EndingScreenController : MonoBehaviour
    {
        private static EndingScreenController instance;
        [SerializeField] private TextMeshProUGUI textPlaceholder;
        [SerializeField] private string[] textItems;
        [SerializeField] private int textInterval = 5;

        private IntervalState textIntervalState;
        private int currentTextIndex = 0;

        public static EndingScreenController GetInstance()
        {
            if (instance != null) return instance;
            instance = Instantiate(AppResource.instance.endingScreenPrefab, Vector3.zero, Quaternion.identity).GetComponent<EndingScreenController>();
            return instance;
        }

        public void Init()
        {
            AppMusic.instance.PlayEndingMusic();
        }

        private void Start()
        {
            textIntervalState = IntervalState.Create(textInterval);
        }

        private void Update()
        {
            if (currentTextIndex < textItems.Length)
            {
                UnityFn.RunWithInterval(this, textIntervalState, () =>
                {
                    textPlaceholder.text = textItems[currentTextIndex];
                    currentTextIndex++;
                });
            }
        }

        public void HandleUpdate(UserInput userInput)
        {
            if (userInput.fire1 || userInput.space)
            {
                AppMusic.instance.Stop();
                SceneManager.LoadScene(0);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}