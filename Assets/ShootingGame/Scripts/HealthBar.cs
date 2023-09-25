using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShooterGame
{
    public class HealthBar : MonoBehaviour
    {

        [SerializeField]
        private Image _backBar;

        [SerializeField]
        private Image _fillBar;

        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }

        public void UpdateHealthbar(int max, int current)
        {
            float maxVal = _backBar.rectTransform.rect.width;
            float newVal = maxVal * current / max;

            _fillBar.rectTransform.sizeDelta = new Vector2(newVal, _fillBar.rectTransform.sizeDelta.y);
        }


    }
}
