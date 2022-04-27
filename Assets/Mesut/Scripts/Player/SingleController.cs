
using System;
using System.Collections;
using DG.Tweening;
using DI;
using UnityEngine;

namespace JR
{
    public enum Gender { Male, Female }
    public class SingleController : MonoBehaviour, IProtector, IGuarded
    {
        [SerializeField] GenderInfo _genderInfo;
        [SerializeField] int _slapAnimationCount = 10;
        IAnimatorController _animatorController;

        public GenderInfo GenderInfo => _genderInfo;

        bool _isSlapping;
        public bool IsSlapping => _isSlapping;

        DoTweenSwapper _transformSwapper;
        ExhaustChecker _exhaustChecker;

        public void Init(InitParameters initParameters)
        {
            _animatorController = initParameters.AnimatorController;

            var swapperInitParameters = new DoTweenSwapper.InitParameters();
            swapperInitParameters.TransformToSwap = transform;
            swapperInitParameters.MoveSettings = initParameters.MoveSettings;
            _transformSwapper = new DoTweenSwapper(swapperInitParameters);

            var exhaustCheckerInitParameters = new ExhaustChecker.InitParameters();
            exhaustCheckerInitParameters.ProtectorController = initParameters.AnimatorController;
            exhaustCheckerInitParameters.Settings = initParameters.ExhaustCheckerSettings;

            _exhaustChecker = new ExhaustChecker();
            _exhaustChecker.Init(exhaustCheckerInitParameters);
        }

        int slapCounter = 0;
        public void Slap(Action action)
        {
            if (_transitionTween != null)
            {
                _transitionTween.Kill();
                _transitionTween = null;
            }
            _animatorController.SetAnimatorSpeed(1.2f);
            _isSlapping = true;
            _animatorController.SetLayerWeight(1, 1f);
            _animatorController.SetTrigger("slap");
            _animatorController.SetFloat("tokatIndex", slapCounter);
            StartCoroutine(RestartAnimatorWeight(action));
            if(!_isIncreasing)
                StartCoroutine(IncreaseCounter());
        }

        Tween _transitionTween;
        bool _isIncreasing;
        IEnumerator IncreaseCounter()
        {
            _isIncreasing = true;
            yield return new WaitForSeconds(1f);
            slapCounter = slapCounter % _slapAnimationCount;
            slapCounter++;
            _isIncreasing = false;
        }
        IEnumerator RestartAnimatorWeight(Action action)
        {
            yield return new WaitForSeconds(0.1f);

            action?.Invoke();
            float timer = 1f;
            _transitionTween = DOTween.To(() => timer, (x) => { timer = x; _animatorController.SetLayerWeight(1, x); }, 0f, 10f)
                .OnComplete(() => { _isSlapping = false; _transitionTween = null; });
            //_animatorController.SetAnimatorSpeed(1f);
        }

        public void Protect()
        {
            _transformSwapper.Swap();
            _exhaustChecker.StartTimer();
            _animatorController.SetTrigger("protectRun");
        }

        public void ReturnBack()
        {
            _transformSwapper.ReturnBack();
            _exhaustChecker.CheckForExhaust();
        }

        public void GetProtected()
        {
            _animatorController.SetTrigger("protectRun");
        }

        public void EndOfProtection()
        {
            _animatorController.SetTrigger("normalRun");
        }

        public class InitParameters
        {
            public IAnimatorController AnimatorController { get; set; }
            public DoTweenSwapper.MoveSettings MoveSettings { get; set; }
            public ExhaustChecker.Settings ExhaustCheckerSettings { get; set; }
            public bool IsProtector { get; set; }
        }

        [System.Serializable]
        public class AnimatorGenderSettings
        {
            [SerializeField] Gender _gender;
            [SerializeField] RuntimeAnimatorController _guardedRuntimeAnimatorController;
            [SerializeField] RuntimeAnimatorController _protecterRunTimeAnimatorController;

            public Gender Gender => _gender;
            public RuntimeAnimatorController GuardedAnimatorController => _guardedRuntimeAnimatorController;
            public RuntimeAnimatorController ProtectorRunTimeAnimatorController => _protecterRunTimeAnimatorController;
        }
    }

    public interface IProtector
    {
        void Protect();
        void ReturnBack();
    }

    public interface IGuarded
    {
        void GetProtected();
        void EndOfProtection();
    }
}