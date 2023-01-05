using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BaseUtil.GameUtil.Domain
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;
        public Gradient gradient;

        private Image fill;

        private void Start()
        {
            fill = slider.fillRect.GetComponent<Image>();
        }

        public void SetMaxHealth(int maxHp)
        {
            slider.maxValue = maxHp;
            slider.value = maxHp;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}