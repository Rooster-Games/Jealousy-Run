using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace JR
{
    public class BarAnimation : MonoBehaviour
    {
        [SerializeField] Transform _handleTransform;
        [SerializeField] Settings _settings;
        bool _isPlaying;

        Tween _animationTween;
        Vector3 _initalScale;

        private void Awake()
        {
            _initalScale = _handleTransform.localScale;
        }

        public void PlayAnimation()
        {
            if (_isPlaying) return;

            _isPlaying = true;

            _handleTransform.DOScale(_handleTransform.localScale * _settings.ScaleMultiplier, _settings.AnimationDuration)
                .SetEase(_settings.Curve)
                .OnComplete(
                () => _handleTransform.DOScale(_initalScale, _settings.AnimationDuration)
                    .SetEase(_settings.Curve)
                    .OnComplete( () => _isPlaying = false));
        }
        [System.Serializable]
        public class Settings
        {
            [SerializeField] float _scaleMultiplier;
            [SerializeField] float _animationDuration = 1f;
            [SerializeField] AnimationCurve _curve;

            public float ScaleMultiplier => _scaleMultiplier;
            public float AnimationDuration => _animationDuration;
            public AnimationCurve Curve => _curve;
        }
    }
}