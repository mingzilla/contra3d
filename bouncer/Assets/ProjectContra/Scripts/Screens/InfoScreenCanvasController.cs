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
        private string title;

        public void Init(string text)
        {
            titleText.GetComponent<Text>().text = text;
        }

        public void HandleUpdate(UserInput userInput, Action runIfStartFn)
        {
            if (userInput.fire1)
            {
                TransitionInToGame();
                runIfStartFn();
            }
        }

        private void TransitionInToGame()
        {
            AppResource.instance.SetControlState(GameControlState.IN_GAME);
            AppResource.instance.infoScreen.SetActive(false);
        }
    }
}