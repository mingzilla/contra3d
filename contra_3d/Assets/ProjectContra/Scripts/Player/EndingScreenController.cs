using BaseUtil.GameUtil;
using BaseUtil.GameUtil.Base;
using BaseUtil.GameUtil.Base.Domain;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using TMPro;
using UnityEngine;

namespace ProjectContra.Scripts.Player
{
    public class EndingScreenController : MonoBehaviour
    {
        private static EndingScreenController instance;
        [SerializeField] private TextMeshProUGUI textPlaceholder;
        [SerializeField] private string[] textItems;
        [SerializeField] private int textInterval = 5;
        [SerializeField] private int delayToStartText = 2;

        private IntervalState textIntervalState;
        private int currentTextIndex = 0;
        private bool canStartText = false;

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
            UnityFn.SetTimeout(this, delayToStartText, () => canStartText = true);
        }

        private void Update()
        {
            if (!canStartText) return;
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
                UnityFn.LoadScene(0);
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}