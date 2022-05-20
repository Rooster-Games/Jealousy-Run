using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCores;
using GameCores.CoreEvents;
using NaughtyAttributes;
using UnityEngine;

namespace JR
{
    public class BarController : MonoBehaviour
    {
        [SerializeField] BarChangeSettings _barChangeSettings;
        [SerializeField] BaseBar _bar;
        [SerializeField] LoveData _loveData;

        [SerializeField] float _testingValue = 0.1f;

        IEventBus _eventBus;
        Tween _increaseTween;
        float _totalIncreaseAmount;
        float _prevIncreaseAmount;
        bool _isBarEmpty;
        BarAnimation _barAnimation;
        // 1
        // 0.1 degisti, current 0.9 oldu,
        // 0.1 degisti, current 0.8 oldu, 
        public void Init(InitParameters initParameters)
        {
            //Debug.Log("IsBarNull: " + _bar == null);
            _bar = initParameters.Bar;
            _loveData = initParameters.LoveData;
            _eventBus = initParameters.EventBus;
            _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
            _barAnimation = _bar.GetComponent<BarAnimation>();
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _bar.gameObject.SetActive(true);
        }

        [Button("ChangeTest")]
        public void ChangeTest()
        {
            ChangeAmount(_testingValue);
        }

        public void ChangeAmount(float amount)
        {
            if (amount > 0f)
                _barAnimation.PlayAnimation();

            _loveData.CurrentValue += amount;
            _bar.ChangeAmount(_loveData.CurrentPercent);

            if (_loveData.CurrentValue <= 0f && !_isBarEmpty)
            {
                _isBarEmpty = true;
                _eventBus.Fire<OnBarEmpty>();
            }

            //_totalIncreaseAmount += amount;
            //_prevIncreaseAmount = _totalIncreaseAmount;

            //if (_increaseTween != null)
            //{
            //    _increaseTween.Kill();
            //    _increaseTween = null;
            //}

            //_increaseTween = DOTween.To(() => _totalIncreaseAmount, (x) => ChangeTotalIncreaseAmount(x), 0f, _barChangeSettings.ChangeDuration * (Mathf.Abs(_totalIncreaseAmount) / 0.1f))
            //    .OnComplete(() => _increaseTween = null)
            //    .SetEase(_barChangeSettings.ChangeCurve);

        }

        private void ChangeTotalIncreaseAmount(float value)
        {
            _totalIncreaseAmount = value;
            float delta = _prevIncreaseAmount - _totalIncreaseAmount;
            _prevIncreaseAmount = _totalIncreaseAmount;
            _loveData.CurrentValue += delta;
            _bar.ChangeAmount(_loveData.CurrentPercent);
        }

        public class InitParameters
        {
            public BaseBar Bar { get; set; }
            public LoveData LoveData { get; set; }
            public IEventBus EventBus { get; set; }
        }

        [System.Serializable]
        public class BarChangeSettings
        {
            [SerializeField] float _changeDuration = 0.25f;
            [SerializeField] AnimationCurve _changeCurve;

            public float ChangeDuration => _changeDuration;
            public AnimationCurve ChangeCurve => _changeCurve;
        }
    }

   
}