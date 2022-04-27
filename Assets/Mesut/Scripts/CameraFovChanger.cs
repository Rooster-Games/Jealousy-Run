using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace JR
{
    public class CameraFovChanger : MonoBehaviour
    {
        CinemachineVirtualCamera _camera;
        Settings _settings;

        Tween _changerTween;
        bool _isStarted;
        public void Init(InitParameters initParameters)
        {
            _camera = initParameters.Camera;
            _settings = initParameters.Settings;
        }

        public void ChangeFovToMax()
        {
            _isStarted = true;

            if (_changerTween != null)
                _changerTween.Kill();

            _changerTween = DOTween.To(() => _camera.m_Lens.FieldOfView, (x) => _camera.m_Lens.FieldOfView = x, _settings.MinMaxFov.y, _settings.ToMaxDuration)
                .SetEase(_settings.ToMaxCurve);
        }

        public void ChangeFovToReturnBack()
        {
            if (!_isStarted) return;

            if (_changerTween != null)
                _changerTween.Kill();

            _changerTween = DOTween.To(() => _camera.m_Lens.FieldOfView, (x) => _camera.m_Lens.FieldOfView = x, _settings.MinMaxFov.x, _settings.ToReturnBackDuration)
                .SetEase(_settings.ToReturnBackCurve);
        }

        public class InitParameters
        {
            public CinemachineVirtualCamera Camera { get; set; }
            public Settings Settings { get; set; }
        }

        [System.Serializable]
        public class Settings
        {
            [MinMaxSlider(50f, 200f)]
            [SerializeField] Vector2 _minMaxFov;
            [Header("=== To Max Settings ===")]
            [SerializeField] float _toMaxDuration = 1f;
            [SerializeField] AnimationCurve _toMaxCurve;

            [Header("=== To Return Back Settings ===")]
            [SerializeField] float _toReturnBackDuration = 1f;
            [SerializeField] AnimationCurve _toReturnBackCurve;

            public Vector2 MinMaxFov => _minMaxFov;
            public float ToMaxDuration => _toMaxDuration;
            public float ToReturnBackDuration => _toReturnBackDuration;
            public AnimationCurve ToMaxCurve => _toMaxCurve;
            public AnimationCurve ToReturnBackCurve => _toReturnBackCurve;
        }
    }
}