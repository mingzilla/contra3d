using System;
using BaseUtil.GameUtil;
using ProjectContra.Scripts.AppSingleton.LiveResource;
using ProjectContra.Scripts.Types;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectContra.Scripts.Screens
{
    public class InfoScreenCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject titleText;
        private Text textComponent;

        private void Start()
        {
            textComponent = titleText.GetComponent<Text>();
        }

        public void UpdateTitleText(string text)
        {
            textComponent.text = text;
        }

        public void HandleUpdate(UserInput userInput, Action runIfStartFn)
        {
            if (userInput.fire1)
            {
                TransitionInToGame(userInput);
                runIfStartFn();
            }
        }

        private void TransitionInToGame(UserInput userInput)
        {
            AppResource.instance.SetControlState(GameControlState.IN_GAME);
            AppResource.instance.infoScreen.SetActive(false);
        }
    }
}