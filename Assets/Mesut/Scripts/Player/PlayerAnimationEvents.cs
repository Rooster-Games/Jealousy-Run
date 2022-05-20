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

        public void Init(InitParameters initParameters)
        {
            _animatorController = initParameters.AnimatorController;
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
            if (_isSetting) return;
            _isSetting = true;
            float timer = _animatorController.GetFloat("tokatIndex");
            float y = timer;
            DOTween.To(() => timer, (x) => { timer = x; _animatorController.SetFloat("tokatIndex", timer); y = timer; }, (timer + 1f) % 6, 0.1f)
                .OnComplete(() => { _animatorController.SetFloat("tokatIndex", Mathf.Ceil(y)); _isSetting = false; });
        }

        private void CheckIsThereEnemy()
        {
            Debug.Log("Checking");
            var colliders = Physics.OverlapSphere(transform.position, 5f, (int)Mathf.Pow(2, 7));

            bool hasMale = false;
            foreach (var collider in colliders)
            {
                var genderInfo = collider.GetComponent<GenderInfo>();
                if (genderInfo == null) continue;

                var gender = genderInfo.Gender;
                hasMale |= gender == Gender.Male;
                if (hasMale)
                    break;
            }

            Debug.Log("HasMale: " + hasMale);

            if(!hasMale)
            {
                Debug.Log("Reseting");
                float timer = 0f;
                DOTween.To(() => timer, (x) => { timer = x; _animatorController.SetLayerWeight(1, x); }, 0f, 0.15f);
                _animatorController.ResetTrigger("slap");
            }
        }
    }

}