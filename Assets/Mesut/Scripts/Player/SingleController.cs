
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
        [SerializeField] Transform[] _handTransformCollection;
        [SerializeField] Vector3 _handScale;

        [SerializeField] GameObject _whileProtectedParticle;
        [SerializeField] GameObject _pushDetector;
        [SerializeField] GameObject _slapDetector;
        [SerializeField] BarController _barController;
        [SerializeField] float _whileProtectingGainLoveDataPerSeconds;

        [SerializeField] List<GameObject> _slapParticlePefabList;

        public GenderInfo GenderInfo => _genderInfo;

        bool _isSlapping;
        public bool IsSlapping => _isSlapping;

        ISwapper _positionSwapper;
        ExhaustChecker _exhaustChecker;

        [SerializeField] SlapParticleRootMarker[] _slapParticleRootMarker;

        WaitForSeconds _gainLoveWaitForSeconds;
        public void Init(InitParameters initParameters)
        {
            _animatorController = initParameters.AnimatorController;
            _animationEvents = initParameters.AnimationEvents;

            _positionSwapper = initParameters.Swapper;


            var exhaustCheckerInitParameters = new ExhaustChecker.InitParameters();
            exhaustCheckerInitParameters.ProtectorController = initParameters.AnimatorController;
            exhaustCheckerInitParameters.Settings = initParameters.ExhaustCheckerSettings;

            _exhaustChecker = new ExhaustChecker();
            _exhaustChecker.Init(exhaustCheckerInitParameters);

            _animationEvents.RegisterOnAnimationEnd(AnimationEvents_OnSlapAnimationEnd);
            _animationEvents.RgisterOnSlap(AnimationEvents_OnSlap);

            _gainLoveWaitForSeconds = new WaitForSeconds(1f);
        }

        bool _isAnimationEnded = true;
        private void AnimationEvents_OnSlapAnimationEnd()
        {
            Debug.Log("Anim Ended");
            //_animatorController.SetLayerWeight(1, 0f);
            _isAnimationEnded = true;
            _isAnimStarted = false;

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
        }


        int slapCounter = 0;

        bool _isAnimStarted;
        public void Slap(Action action)
        {
            //if (!_isAnimStarted)
            //{
            //    _isAnimStarted = true;
            //    Debug.Log("Anim Started");
            //}

            //if (!_isAnimationEnded)
            //{
            //    Debug.Log("Not Ended returned");
            //    return;
            //}

            //_slapDetector.SetActive(false);
            _isAnimationEnded = false;
            //if (_transitionTween != null)
            //{
            //    _transitionTween.Kill();
            //    _transitionTween = null;
            //}
            //_animatorController.SetAnimatorSpeed(1.2f);
            _isSlapping = true;
            _animatorController.SetLayerWeight(1, 1f);
            _animatorController.SetTrigger("slap");
            //_animatorController.SetFloat("tokatIndex", slapCounter);
            //StartCoroutine(RestartAnimatorWeight(action));
               //StartCoroutine(IncreaseCounter(action));
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
            _animatorController.SetTrigger("protectRun");
            StartCoroutine(GainLove());
        }

        public void ReturnBack()
        {
            _slapDetector.SetActive(false);
            _positionSwapper.ReturnBack();
            _exhaustChecker.CheckForExhaust();
            _isProtecting = false;

            _animatorController.SetLayerWeight(1, 0f);
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
            public DoTweenSwapper.MoveSettings MoveSettings { get; set; }
            public ExhaustChecker.Settings ExhaustCheckerSettings { get; set; }
            public PlayerAnimationEvents AnimationEvents { get; set; }
            public ISwapper Swapper { get; set; }
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
        void MakeHandsBigger();
    }

    public interface IGuarded
    {
        void GetProtected();
        void EndOfProtection();
    }
}