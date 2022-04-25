using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCores;
using GameCores.CoreEvents;
using System.Runtime;
using DG.Tweening;
using UnityEditor.ShaderGraph.Serialization;
using System;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;

namespace JR
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Transform[] _coupleTransform;

        Settings _settings;
        DollyCartController _dollyCartController;
        IAnimatorController _animatorController;
        EventBus _eventBus;
        InputManager _inputManager;

        IProtector _protector;
        // bool _swapping;

        private const int COUPLE_COUNTT = 2;

        Vector3[] _moveForwardPoses;
        bool _isInitialized;

        public void Init(InitParameters initParameters)
        {
            _dollyCartController = initParameters.DollyCartController;
            _animatorController = initParameters.AnimatorController;
            _settings = initParameters.Settings;
            _eventBus = initParameters.EventBus;
            _inputManager = initParameters.InputManager;
            _protector = initParameters.Protector;
            _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);


            _moveForwardPoses = new Vector3[COUPLE_COUNTT] { new Vector3(0f, 0f, -_settings.MoveForwardDistance), new Vector3(0f, 0f, _settings.MoveForwardDistance) };
            _isInitialized = true;
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _dollyCartController.StartMoving();
            _animatorController.SetTrigger("walk");
        }

        //Tween[] _swapTweens = new Tween[4];
        //public void SwapPositions()
        //{
        //    _swappedOne = true;
        //    var forwardPositionList = new List<Vector3>();
        //    forwardPositionList.AddRange(_moveForwardPoses);

        //    _swapping = true;
        //    var targetPoses = new Vector3[_coupleTransform.Length];
        //    targetPoses[0] = _coupleTransform[1].localPosition;
        //    targetPoses[1] = _coupleTransform[0].localPosition;

        //    for (int i = 0; i < _coupleTransform.Length; i++)
        //    {
        //        var randomIndex = Random.Range(0, forwardPositionList.Count);
        //        var forwardPosition = forwardPositionList[randomIndex];
        //        forwardPositionList.RemoveAt(randomIndex);

        //        _swapTweens[i] = _coupleTransform[i].DOLocalMoveX(targetPoses[i].x, _settings.SwapDuration)
        //            .SetEase(_settings.SideSwapEase)
        //            .OnComplete( () => _swapping = false);

        //        _swapTweens[i + 2] = _coupleTransform[i].DOLocalMoveZ(forwardPosition.z, _settings.SwapDuration)
        //            .SetEase(_settings.ForwardCurve);

        //        _swapTweens[i].SetAutoKill(false);
        //        _swapTweens[i + 2].SetAutoKill(false);
        //    }
        //}

        //private void FlipSwapTweens()
        //{
        //    foreach (var swapTween in _swapTweens)
        //    {
        //        swapTween.Flip();
        //        swapTween.OnRewind(() => _swapping = false);
        //    }
        //}

        //private void PlayBackWardsSwapTweens()
        //{
        //    _swapping = true;
        //    foreach (var swapTween in _swapTweens)
        //    {
        //        swapTween.PlayBackwards();
        //        swapTween.OnRewind(() => _swapping = false);
        //    }
        //}

        //bool _swappedOne;

        private void Update()
        {
            if (!_isInitialized) return;

            if(_inputManager.IsMouseButtonDown)
            {
                _protector.Protect();
            }

            if(_inputManager.IsMouseButtonUp)
            {
                _protector.ReturnBack();
            }
        }

        //private void Update()
        //{
        //    if( _inputManager.IsMouseButtonDown )
        //    {
        //        if (!_swapping)
        //            SwapPositions();
        //        else if (_swapping)
        //            FlipSwapTweens();
        //    }

        //    if(_inputManager.IsMouseButtonUp)
        //    {
        //        if (_swapping)
        //            FlipSwapTweens();
        //        else if (!_swapping)
        //        {
        //            if (!_swappedOne) return;

        //            PlayBackWardsSwapTweens();
        //        }
        //    }
        //}


        [System.Serializable]
        public class Settings
        {
            [SerializeField] float _swapDuration = 1f;
            [SerializeField] float _moveForwardDistance = 1f;
            [SerializeField] AnimationCurve _forwardCurve;
            [SerializeField] Ease _sideSwapEase = Ease.Linear;

            public float SwapDuration => _swapDuration;
            public float MoveForwardDistance => _moveForwardDistance;
            public AnimationCurve ForwardCurve => _forwardCurve;
            public Ease SideSwapEase => _sideSwapEase;
        }

        public class InitParameters
        {
            public DollyCartController DollyCartController { get; set; }
            public IAnimatorController AnimatorController { get; set; }
            public Settings Settings { get; set; }
            public EventBus EventBus { get; set; }
            public InputManager InputManager { get; set; }
            public IProtector Protector { get; set; }
        }
    }

    public class DoTweenSwapper
    {
        Transform _transformToSwap;
        MoveSettings _moveSettings;

        Tween _forwardMoveTween;
        Tween _sideMoveTween;

        bool _isSwapping;

        public Tween SideMoveTween => _sideMoveTween;
        public Tween ForwardMoveTween => _forwardMoveTween;

        public DoTweenSwapper(InitParameters initParameters)
        {
            _transformToSwap = initParameters.TransformToSwap;
            _moveSettings = initParameters.MoveSettings;

        }

        bool _isInitialized;
        private void Init()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            _forwardMoveTween = _transformToSwap.DOLocalMoveZ(_moveSettings.MoveForwardDistance, _moveSettings.MoveDuration)
                .SetEase(_moveSettings.ForwardMoveCurve);
            _sideMoveTween = _transformToSwap.DOLocalMoveX(_moveSettings.MoveSideDistance, _moveSettings.MoveDuration)
                .OnComplete(() => {_isSwapping = false; });

            _sideMoveTween.OnRewind(() => {_isSwapping = false;});

            _forwardMoveTween.SetAutoKill(false);
            _sideMoveTween.SetAutoKill(false);
        }

        public void Swap()
        {

            if (!_isInitialized)
            {
                Init();
                return;
            }

            if (!_isSwapping)
            {
                _isSwapping = true;
                _forwardMoveTween.PlayForward();
                _sideMoveTween.PlayForward();

                _sideMoveTween.OnComplete(() => { _isSwapping = false; });
            }
            else
            {
                FlipTweens();
            }
        }

        public void ReturnBack()
        {
            if (_isSwapping)
            {
                FlipTweens();
            }
            else
            {
                PlayBackwardsTweens();
            }
        }

        private void FlipTweens()
        {
            _forwardMoveTween.Flip();
            _sideMoveTween.Flip();

            _sideMoveTween.OnRewind(() => { _isSwapping = false; });
        }

        private void PlayBackwardsTweens()
        {
            _isSwapping = true;
            _forwardMoveTween.PlayBackwards();
            _sideMoveTween.PlayBackwards();

            _sideMoveTween.OnRewind(() => { _isSwapping = false; });

            _forwardMoveTween.SetAutoKill(false);
            _sideMoveTween.SetAutoKill(false);
        }

        public class InitParameters
        {
            public Transform TransformToSwap { get; set; }
            public MoveSettings MoveSettings { get; set; }
        }

        [System.Serializable]
        public class MoveSettings
        {
            [SerializeField] float _moveSideDistance = -1f;
            [SerializeField] float _moveForwardDistance = 1f;
            [SerializeField] float _moveDuration = 1f;
            [SerializeField] AnimationCurve _forwardMoveCurve;

            public float MoveSideDistance => _moveSideDistance;
            public float MoveForwardDistance => _moveForwardDistance;
            public float MoveDuration => _moveDuration;
            public AnimationCurve ForwardMoveCurve => _forwardMoveCurve;
        }
    }
}
