using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UIThings
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] Sprite _cursorImage;

        [SerializeField] CursorClickSettings _settings;


        bool _isScaling;
        float _startingScale;

        private void Start()
        {

            var cursorImage = _settings.CursorTransform.GetComponent<Image>();
            if (_cursorImage != null)
                cursorImage.sprite = _cursorImage;

            _startingScale = _settings.CursorTransform.localScale.x;
        }

        private void Update()
        {
            if(_settings.CanMove)
                _settings.CursorTransform.position = Input.mousePosition + _settings.CursorPositionOffset;

            if(Input.GetMouseButtonDown(0) && !_isScaling)
            {
                StartCoroutine(OnClick());
            }

        }

        //private void OnClick()
        //{
        //    if (_isScaling) return;
        //    _isScaling = true;

        //    _settings.CursorTransform.DOScale(_settings.TargetScale, _settings.ScaleDuration)
        //        .SetLoops(2, LoopType.Yoyo)
        //        .OnComplete(() => _isScaling = false);
        //}

        IEnumerator OnClick()
        {
            _isScaling = true;

            float timer = 0f;

            while (timer < _settings.ScaleDuration)
            {
                timer += Time.deltaTime;

                float percent = timer / _settings.ScaleDuration;
                float curvePercent = _settings.ScaleCurve.Evaluate(percent);
                float scale = Mathf.Lerp(_startingScale, _settings.TargetScale, curvePercent);

                _settings.CursorTransform.localScale = Vector3.one * scale;

                yield return null;
            }

            _isScaling = false;
        }

        [System.Serializable]
        public class CursorClickSettings
        {
            [field: SerializeField] public bool CanMove { get; private set; } = true;
            [field: SerializeField] public Transform CursorTransform { get; private set; }
            [field: SerializeField] public float ScaleDuration { get; private set; } = 0.25f;
            [field: SerializeField] public float TargetScale { get; private set; } = 0.45f;
            [field: SerializeField] public Vector3 CursorPositionOffset { get; private set; }
            [field: SerializeField] public AnimationCurve ScaleCurve { get; private set; }
        }
    }
}
