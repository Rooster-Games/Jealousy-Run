
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCores;
using GameCores.CoreEvents;
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
        [SerializeField] Transform[] _handTransformCollection;
        [SerializeField] Vector3 _handScale;

        [SerializeField] GameObject _whileProtectedParticle;
        [SerializeField] GameObject _pushDetector;
        [SerializeField] GameObject _slapDetector;
        [SerializeField] BarController _barController;

        float _whileProtectingGainLoveDataPerSeconds;

        [SerializeField] List<GameObject> _slapParticlePefabList;

        public GenderInfo GenderInfo => _genderInfo;

        bool _isSlapping;
        public bool IsSlapping => _isSlapping;

        ISwapper _positionSwapper;
        ExhaustChecker _exhaustChecker;
        IEventBus _eventBus;

        [SerializeField] SlapParticleRootMarker[] _slapParticleRootMarker;

        WaitForSeconds _gainLoveWaitForSeconds;
        public void Init(InitParameters initParameters)
        {
            _animatorController = initParameters.AnimatorController;
            _animationEvents = initParameters.AnimationEvents;

            _positionSwapper = initParameters.Swapper;
            _exhaustChecker = initParameters.ExhaustChecker;
            _eventBus = initParameters.EventBus;
            _genderInfo = initParameters.GenderInfo;
            _whileProtectingGainLoveDataPerSeconds = initParameters.BarChangingSettings.IncreaseSettings.WhileProtectingGainPerSeconds;

            _animationEvents.RegisterOnAnimationEnd(AnimationEvents_OnSlapAnimationEnd);
            _animationEvents.RgisterOnSlap(AnimationEvents_OnSlap);

            _gainLoveWaitForSeconds = new WaitForSeconds(1f);

            _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
            _eventBus.Register<OnBarEmpty>(EventBus_OnBarEmpty);

            initParameters.CheckNullField();
        }

        private void EventBus_OnBarEmpty(OnBarEmpty eventData)
        {
            _animatorController.SetTrigger("lose");
            _animatorController.SetLayerWeight(1, 0f);
            StopAllCoroutines();
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _animatorController.SetTrigger("normalRun");
        }

        bool _isAnimationEnded = true;
        private void AnimationEvents_OnSlapAnimationEnd()
        {
            //_animatorController.SetLayerWeight(1, 0f);
            _isAnimationEnded = true;

            //_slapDetector.SetActive(true);
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

            _eventBus.Fire<OnSlap>();
        }

        public void Slap()
        {
            _isAnimationEnded = false;
            _isSlapping = true;
            _animatorController.SetLayerWeight(1, 1f);
            _animatorController.SetTrigger("slap");

            ResetEndSlapTweens();
            TryToEndSlapAnimation();
        }

        Tween _endCheckerTween;
        Tween _weightDecreaserTween;

        private void ResetEndSlapTweens()
        {
            ResetTween(ref _endCheckerTween);
            ResetTween(ref _weightDecreaserTween);
        }

        private void ResetTween(ref Tween tween)
        {
            if(tween != null)
            {
                tween.Kill();
                tween = null;
            }
        }

        private void TryToEndSlapAnimation()
        {
            float timer = 0f;
            _endCheckerTween = DOTween.To(() => timer, (x) => timer = x, 1f, 0.5f)
                .OnComplete(SetLayerWeight);
        }

        private void SetLayerWeight()
        {
            float weight = 1f;
            _weightDecreaserTween = DOTween.To(() => weight, (x) => { weight = x; _animatorController.SetLayerWeight(1, x); }, 0f, 0.35f);
        }

        bool _isProtecting;
        IEnumerator GainLove()
        {
            _isProtecting = true;
            float timer = 0f;
            while(_isProtecting)
            {
                timer += Time.deltaTime;
                if (timer >= 1f)
                {
                    _barController.ChangeAmount(_whileProtectingGainLoveDataPerSeconds);
                    timer = 0f;
                }
                yield return null;
            }

            _barController.ChangeAmount(_whileProtectingGainLoveDataPerSeconds * timer);
        }

        public void Protect()
        {
            _slapDetector.SetActive(true);
            _positionSwapper.Swap();
            _exhaustChecker.StartTimer();
            _animatorController.SetTrigger("turnLeft");
            StartCoroutine(GainLove());

            if (_checkWayPercentCO != null)
                StopCoroutine(_checkWayPercentCO);
            _checkWayPercentCO = StartCoroutine(CheckWayPercent(_positionSwapper, 0.75f, () => _animatorController.SetTrigger("protectRun")));
        }
        Coroutine _checkWayPercentCO;
        IEnumerator CheckWayPercent(ISwapper swapper, float checkThreshold, Action action)
        {
            float timer = 0f;
            while(swapper.WayPercent < checkThreshold)
            {
                timer += Time.deltaTime;
                if (timer > 1.5f && _checkWayPercentCO != null)
                    StopCoroutine(_checkWayPercentCO);

                yield return null;
            }

            action?.Invoke();
        }

        bool _exhausted;
        public void ReturnBack()
        {
            _animatorController.ResetTrigger("turnLeft");
            _animatorController.ResetTrigger("protectRun");

            _exhausted = false;
            if (_checkWayPercentCO != null)
                StopCoroutine(_checkWayPercentCO);
            _checkWayPercentCO = StartCoroutine(CheckWayPercent(_positionSwapper, 1f, () => { _exhausted = true; _exhaustChecker.CheckForExhaust(); }));

            _slapDetector.SetActive(false);
            _positionSwapper.ReturnBack();
            //_exhaustChecker.CheckForExhaust();
            _isProtecting = false;

            _animatorController.SetLayerWeight(1, 0f);


            if(_positionSwapper.WayPercent > 0.75f)
            {
                _animatorController.SetTrigger("turnRight");
            }
            if(!_exhausted)
                _animatorController.SetTrigger("normalRun");

            _animatorController.SetLayerWeight(1, 0f);
            _animatorController.SetTrigger("slapEnd");
        }

        public void GetProtected()
        {
            _animatorController.SetTrigger("protectRun");
            _whileProtectedParticle.SetActive(true);
            _pushDetector.SetActive(false);

        }

        public void EndOfProtection()
        {
            _animatorController.SetTrigger("normalRun");
            _whileProtectedParticle.SetActive(false);
            _pushDetector.SetActive(true);
        }

        public void MakeHandsBigger()
        {
            foreach (var handTransform in _handTransformCollection)
            {
                handTransform.localScale = _handScale;
            }
        }

        public class InitParameters
        {
            public IAnimatorController AnimatorController { get; set; }
            public PlayerAnimationEvents AnimationEvents { get; set; }
            public ISwapper Swapper { get; set; }
            public IEventBus EventBus { get; set; }
            public GenderInfo GenderInfo { get; set; }
            public ExhaustChecker ExhaustChecker { get; set; }
            public BarChangingSettings BarChangingSettings { get; set; }
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
        void Slap();
        void Protect();
        void ReturnBack();
        void MakeHandsBigger();
    }

    public interface IGuarded
    {
        void GetProtected();
        void EndOfProtection();
    }
}