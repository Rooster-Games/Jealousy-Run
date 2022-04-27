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
        Tween _sideChangerTween;
        bool _isStarted;
        CinemachineTransposer _transposer;
        Vector3 _transposerInitialOffset;
        public void Init(InitParameters initParameters)
        {
            _camera = initParameters.Camera;
            _settings = initParameters.Settings;
            _transposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
            _transposerInitialOffset = _transposer.m_FollowOffset;
        }

        public void ChangeFovToMax()
        {
            _isStarted = true;

            if (_changerTween != null)
                _changerTween.Kill();

            _changerTween = DOTween.To(() => _camera.m_Lens.FieldOfView, (x) => _camera.m_Lens.FieldOfView = x, _settings.MinMaxFov.y, _settings.ToMaxDuration)
                .SetEase(_settings.ToMaxCurve);

            ChangeToLeft();
        }

        public void ChangeFovToReturnBack()
        {
            if (!_isStarted) return;

            if (_changerTween != null)
                _changerTween.Kill();

            ChangeToRight();

            _changerTween = DOTween.To(() => _camera.m_Lens.FieldOfView, (x) => _camera.m_Lens.FieldOfView = x, _settings.MinMaxFov.x, _settings.ToReturnBackDuration)
                .SetEase(_settings.ToReturnBackCurve);
        }

        private void ChangeToLeft()
        {
            if (_sideChangerTween != null)
                _sideChangerTween.Kill();

            var targetOffset = _transposerInitialOffset + new Vector3(_settings.TargetXOffset, 0f, 0f);
            _sideChangerTween = DOTween.To(() => _transposer.m_FollowOffset, (x) => _transposer.m_FollowOffset = x, targetOffset, _settings.ReachToTargetOffsetDuration)
                .SetEase(_settings.SideMovementCurve);
        }

        private void ChangeToRight()
        {
            if (_sideChangerTween != null)
                _sideChangerTween.Kill();

            _sideChangerTween = DOTween.To(() => _transposer.m_FollowOffset, (x) => _transposer.m_FollowOffset = x, _transposerInitialOffset, _settings.ReachToTargetOffsetDuration)
                .SetEase(_settings.SideMovementCurve);
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

            [Header("=== Side Movement Settings ===")]
            [SerializeField] float _targetXOffset = -1.6f;
            [SerializeField] float _reachToTargetOffsetDuration = 1f;
            [SerializeField] AnimationCurve _sideMovementCurve;

            public Vector2 MinMaxFov => _minMaxFov;
            public float ToMaxDuration => _toMaxDuration;
            public float ToReturnBackDuration => _toReturnBackDuration;
            public AnimationCurve ToMaxCurve => _toMaxCurve;
            public AnimationCurve ToReturnBackCurve => _toReturnBackCurve;

            public float TargetXOffset => _targetXOffset;
            public float ReachToTargetOffsetDuration => _reachToTargetOffsetDuration;
            public AnimationCurve SideMovementCurve => _sideMovementCurve;
        }
    }
}