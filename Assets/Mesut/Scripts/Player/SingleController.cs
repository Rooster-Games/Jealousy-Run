
using System;
using System.Collections;
using System.Collections.Generic;
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
        PlayerAnimationEvents _animationEvents;

        [SerializeField] List<GameObject> _slapParticlePefabList;

        public GenderInfo GenderInfo => _genderInfo;

        bool _isSlapping;
        public bool IsSlapping => _isSlapping;

        DoTweenSwapper _transformSwapper;
        ExhaustChecker _exhaustChecker;

        [SerializeField] SlapParticleRootMarker[] _slapParticleRootMarker;

        public void Init(InitParameters initParameters)
        {
            _animatorController = initParameters.AnimatorController;
            _animationEvents = initParameters.AnimationEvents;

            var swapperInitParameters = new DoTweenSwapper.InitParameters();
            swapperInitParameters.TransformToSwap = transform;
            swapperInitParameters.MoveSettings = initParameters.MoveSettings;
            _transformSwapper = new DoTweenSwapper(swapperInitParameters);

            var exhaustCheckerInitParameters = new ExhaustChecker.InitParameters();
            exhaustCheckerInitParameters.ProtectorController = initParameters.AnimatorController;
            exhaustCheckerInitParameters.Settings = initParameters.ExhaustCheckerSettings;

            _exhaustChecker = new ExhaustChecker();
            _exhaustChecker.Init(exhaustCheckerInitParameters);

            _animationEvents.RegisterOnAnimationEnd(AnimationEvents_OnSlapAnimationEnd);
            _animationEvents.RgisterOnSlap(AnimationEvents_OnSlap);
        }

        bool _isAnimationEnded = true;
        private void AnimationEvents_OnSlapAnimationEnd()
        {
            _animatorController.SetLayerWeight(1, 0f);
            _isAnimationEnded = true;
        }

        int _particleCounter;
        private void AnimationEvents_OnSlap()
        {
            int randomIndex = UnityEngine.Random.Range(0, _slapParticlePefabList.Count);
            var prefab = _slapParticlePefabList[randomIndex];
            var go = Instantiate(prefab);

            //go.transform.SetParent(_slapParticleRootMarker.transform);
            go.transform.position = _slapParticleRootMarker[_particleCounter++ % 2].transform.position + transform.forward * 3f;
            go.transform.localScale = Vector3.one * 2f;

            Destroy(go, 3f);
        }


        int slapCounter = 0;
        public void Slap(Action action)
        {
            if (!_isAnimationEnded) return;

            _isAnimationEnded = false;
            if (_transitionTween != null)
            {
                _transitionTween.Kill();
                _transitionTween = null;
            }
            _animatorController.SetAnimatorSpeed(1.2f);
            _isSlapping = true;
            _animatorController.SetLayerWeight(1, 1f);
            _animatorController.SetTrigger("slap");
            //_animatorController.SetFloat("tokatIndex", slapCounter);
            //StartCoroutine(RestartAnimatorWeight(action));
               StartCoroutine(IncreaseCounter(action));
        }



        Tween _transitionTween;
        //bool _isIncreasing;
        IEnumerator IncreaseCounter(Action action)
        {
            yield return new WaitForSeconds(0.25f);
            action?.Invoke();
            //_isIncreasing = true;
            //yield return new WaitForSeconds(0.5f);
            //slapCounter = slapCounter % _slapAnimationCount;
            //slapCounter++;
            //_isIncreasing = false;
        }
        IEnumerator RestartAnimatorWeight(Action action)
        {
            yield return new WaitForSeconds(0.1f);

            action?.Invoke();
            float timer = 1f;
            _transitionTween = DOTween.To(() => timer, (x) => { timer = x; _animatorController.SetLayerWeight(1, x); }, 0f, 0.25f)
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
            public PlayerAnimationEvents AnimationEvents { get; set; }
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