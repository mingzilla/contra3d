using System;
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
    }
}