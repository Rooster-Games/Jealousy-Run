using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoosterHub
{
    public class PercentageProgression : MonoBehaviour
    {
        public static Action OnPercentageProgressionComplete;
        private static Action<float> OnPercentageProgressionUpdate;

        [SerializeField] private TextMeshProUGUI percentageText;
        [SerializeField] private Image fillBar;

        private const string percentageValue = "PERCENTAGE_VALUE";

        private void OnEnable()
        {
            var prefVal = PlayerPrefs.GetFloat(percentageValue);
            if (Math.Abs(prefVal - 1) < .02f)
            {
                PlayerPrefs.SetFloat(percentageValue,0);
            }
            
            fillBar.DOFillAmount(prefVal, 0f);
            percentageText.text = "%" + Mathf.RoundToInt(prefVal * 100);
            OnPercentageProgressionUpdate += FillBar;
        }

        private void OnDisable()
        {
            OnPercentageProgressionUpdate -= FillBar;
        }

        private void FillBar(float value)
        {
            var prefVal = PlayerPrefs.GetFloat(percentageValue);

            fillBar.DOFillAmount(value + prefVal, 1.2f).SetEase(Ease.InOutCirc);
            percentageText.text = "%" + Mathf.RoundToInt((value + prefVal) * 100);
            
            PlayerPrefs.SetFloat(percentageValue, value + prefVal);

            if (Math.Abs(PlayerPrefs.GetFloat(percentageValue) - 1) < .02f)
            {
                OnPercentageProgressionComplete?.Invoke();
            }
        }

        public static void IncreasePercentage(float value)
        {
            OnPercentageProgressionUpdate?.Invoke(value);
        }
    }
}