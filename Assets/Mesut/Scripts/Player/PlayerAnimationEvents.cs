using System;
using System.Collections;
using System.Collections.Generic;
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
            _animatorController.SetFloat("tokatIndex", _slapCounter % 8);
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
    }
}