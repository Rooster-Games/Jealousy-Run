using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace JR
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        Action OnAnimationEnd;
        Action OnSlap;
        IAnimatorController _animatorController;
        Settings _settings;

        public void Init(InitParameters initParameters)
        {
            _animatorController = initParameters.AnimatorController;
            _settings = initParameters.Settings;
        }

        int _slapCounter;

        public void SlapAnimationEnd()
        {
            _slapCounter++;
            //_animatorController.SetFloat("tokatIndex", _slapCounter % 8);
            SetSlapIndex();
            OnAnimationEnd?.Invoke();
        }

        public void Slap()
        {
            OnSlap?.Invoke();
        }

        public class InitParameters
        {
            public IAnimatorController AnimatorController { get; set; }
            public Settings Settings { get; set; }
        }

        public void RegisterOnAnimationEnd(Action action)
        {
            OnAnimationEnd += action;
        }
        public void RgisterOnSlap(Action action)
        {
            OnSlap += action;
        }

        bool _isSetting;
        private void SetSlapIndex()
        {
            //if (_isSetting) return;
            //_isSetting = true;
            float timer = _animatorController.GetFloat("tokatIndex");
            // _animatorController.SetFloat("tokatIndex", (timer + 1f) % 6);
            float y = timer;
            DOTween.To(() => timer, (x) => { timer = x; _animatorController.SetFloat("tokatIndex", timer); y = timer; }, (timer + 1f) % _settings.SlapAnimationCount, 0.25f)
                .OnComplete(() => { _animatorController.SetFloat("tokatIndex", Mathf.Ceil(y)); _isSetting = false; });
        }

        [System.Serializable]
        public class Settings
        {
            [field: SerializeField] public int SlapAnimationCount { get; private set; }
        }
    }
}