using System;
using BaseUtil.GameUtil;
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
        [SerializeField] private GameObject titleText;
        private string title;

        public void Init(string text)
        {
            titleText.GetComponent<Text>().text = text;
        }

        public void HandleUpdate(UserInput userInput)
        {
            if (userInput.fire1)
            {
                AppResource.instance.SetControlState(GameControlState.IN_GAME);
            }
        }
    }
}