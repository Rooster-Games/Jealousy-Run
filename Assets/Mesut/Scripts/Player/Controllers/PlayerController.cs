
using UnityEngine;
using GameCores;
using GameCores.CoreEvents;
using DG.Tweening;
using NaughtyAttributes;
using DI;

namespace JR
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Transform[] _coupleTransform;
        [SerializeField] GameObject _speedUpParticle;
        DollyCartController _dollyCartController;
        IAnimatorController _animatorController;
        EventBus _eventBus;
        InputManager _inputManager;

        IProtector _protector;
        IGuarded _guarded;
        SpeedChanger _speedChanger;
        CameraFovChanger _cameraFovChanger;
        // bool _swapping;

        bool _isInitialized;

        public void Init(InitParameters initParameters)
        {
            _dollyCartController = initParameters.DollyCartController;
            _animatorController = initParameters.AnimatorController;
            _eventBus = initParameters.EventBus;
            _inputManager = initParameters.InputManager;
            _protector = initParameters.Protector;
            _guarded = initParameters.Guarded;
            _cameraFovChanger = initParameters.CameraFovChanger;

            _isInitialized = true;

            var speedChangerInitParameters = new SpeedChanger.InitParameters();
            speedChangerInitParameters.DollyCartController = _dollyCartController;
            speedChangerInitParameters.Settings = initParameters.SpeedChangerSettings;

            _speedChanger = new SpeedChanger();
            _speedChanger.Init(speedChangerInitParameters);

            _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _dollyCartController.StartMoving();
            _animatorController.SetTrigger("normalRun");
        }

        private void Update()
        {
            if (!_isInitialized) return;

            if(_inputManager.IsMouseButtonDown)
            {
                _protector.Protect();
                _guarded.GetProtected();
                _speedChanger.SpeedUp();
                _cameraFovChanger.ChangeFovToMax();
                _speedUpParticle.SetActive(true);
            }

            if(_inputManager.IsMouseButtonUp)
            {
                _protector.ReturnBack();
                _guarded.EndOfProtection();
                _speedChanger.SpeedDown();
                _cameraFovChanger.ChangeFovToReturnBack();
                _speedUpParticle.SetActive(false);
            }
        }


        //[System.Serializable]
        //public class Settings
        //{
        //    [SerializeField] float _swapDuration = 1f;
        //    [SerializeField] float _moveForwardDistance = 1f;
        //    [SerializeField] AnimationCurve _forwardCurve;
        //    [SerializeField] Ease _sideSwapEase = Ease.Linear;

        //    public float SwapDuration => _swapDuration;
        //    public float MoveForwardDistance => _moveForwardDistance;
        //    public AnimationCurve ForwardCurve => _forwardCurve;
        //    public Ease SideSwapEase => _sideSwapEase;
        //}

        public class InitParameters
        {
            public DollyCartController DollyCartController { get; set; }
            public IAnimatorController AnimatorController { get; set; }
            //public Settings Settings { get; set; }
            public EventBus EventBus { get; set; }
            public InputManager InputManager { get; set; }
            public IProtector Protector { get; set; }
            public IGuarded Guarded { get; set; }
            public SpeedChanger.Settings SpeedChangerSettings { get; set; }
            public CameraFovChanger CameraFovChanger { get; set; }
        }
    }

    public class ExhaustChecker
    {
        IAnimatorController _protectorController;
        Settings _settings;

        bool _isExhausted;

        float _timer;
        Tween _timerTween;
        Tween _becomeExhaust;
        public void Init(InitParameters initParameters)
        {
            _protectorController = initParameters.ProtectorController;
            _settings = initParameters.Settings;
        }

        public void StartTimer()
        {
            if (_becomeExhaust != null)
                _becomeExhaust.Kill();

            float timer = _timer;
            _timerTween = DOTween.To(() => timer, (x) => { timer = x; _timer = x; }, _settings.BecomeExhaustSeconds, _settings.BecomeExhaustSeconds)
                .OnComplete(() => { _isExhausted = true;});
        }

        public void CheckForExhaust()
        {
            if (_isExhausted)
                BecomeExhaust();
            else
            {
                _protectorController.SetTrigger("turnRight");
                _protectorController.SetTrigger("normalRun");
                _timerTween.Kill();
            }
        }

        private void BecomeExhaust()
        {
            _protectorController.SetTrigger("turnRight");
            _protectorController.SetTrigger("yorgun");

            float timer = _timer;
            _becomeExhaust = DOTween.To(() => timer, (x) => { timer = x; _timer = x; }, 0f, _settings.ExhaustDuration)
                .OnComplete(CoolDown);
        }

        private void CoolDown()
        {
            _isExhausted = false;
            _protectorController.SetTrigger("normalRun");
        }

        public class InitParameters
        {
            public IAnimatorController ProtectorController { get; set; }
            public Settings Settings { get; set; }
        }

        [System.Serializable]
        public class Settings
        {
            [SerializeField] float _becomeExhaustSeconds = 5f;
            [SerializeField] float _exhaustDuration = 1f;

            public float BecomeExhaustSeconds => _becomeExhaustSeconds;
            public float ExhaustDuration => _exhaustDuration;
        }
    }

    public class SpeedChanger
    {
        Settings _settings;
        DollyCartController _dollyCartController;

        private float _initialSpeed;
        private float _targetSpeed;
        private float _currentSpeed;

        private float _initialDeltaChange;

        Tween _speedChangeTween;

        private bool _isSpeedUpStarted;

        public void Init(InitParameters initParameters)
        {
            _settings = initParameters.Settings;
            _dollyCartController = initParameters.DollyCartController;

            _initialSpeed = _dollyCartController.StartingSpeed;
            _targetSpeed = _initialSpeed + _initialSpeed * _settings.SpeedUpPercent;
            _initialDeltaChange = _targetSpeed - _initialSpeed;
        }

        public void SpeedUp()
        {
            _isSpeedUpStarted = true;
            _currentSpeed = _dollyCartController.CurrentSpeed;
            float durationPercent = (_targetSpeed - _currentSpeed) / _initialDeltaChange;

            if (_speedChangeTween != null)
                _speedChangeTween.Kill();

            _speedChangeTween = DOTween.To(() => _currentSpeed, (x) => _currentSpeed = x, _targetSpeed, _settings.SpeedUpDuration * durationPercent)
                .SetEase(_settings.SpeedUpCurve)
                .OnUpdate(ChangeCurrentSpeed)
                .OnComplete(SpeedUpFrequently);
        }

        public void SpeedDown()
        {
            if (!_isSpeedUpStarted) return;

            if (_speedChangeTween != null)
                _speedChangeTween.Kill();

            float durationPercent = (_currentSpeed - _initialSpeed) / _initialDeltaChange;

            _speedChangeTween = DOTween.To(() => _currentSpeed, (x) => _currentSpeed = x, _initialSpeed, _settings.ReturnBackDuration * durationPercent)
                .SetEase(_settings.RetrunBackCurve)
                .OnUpdate(ChangeCurrentSpeed);
        }

        private void SpeedUpFrequently()
        {
            _speedChangeTween = DOTween.To(() => _currentSpeed, (x) => _currentSpeed = x, (_currentSpeed + _settings.AfterHalfSpeedUpValue), _settings.AfterHalfSeedpUpEverySeconds)
                .OnUpdate(() => { ChangeCurrentSpeed(); })
                .SetLoops(-1, LoopType.Incremental);

        }

        private void ChangeCurrentSpeed()
        {
            _dollyCartController.ChangeCurrentSpeed(_currentSpeed);
        }

        public class InitParameters
        {
            public DollyCartController DollyCartController { get; set; }
            public Settings Settings { get; set; }
        }

        [System.Serializable]
        public class Settings
        {
            [Header("=== % Speed Up Settings ===")]
            [Range(0f, 1f)]
            [SerializeField] float _speedUpPercent = 0.5f;
            [SerializeField] float _speedUpDuration = 1f;
            [SerializeField] AnimationCurve _speedUpCurve;

            [Header("=== After Reaching % Speed Up Settings ===")]
            [SerializeField] float _afterHalfSpeedUpEverySeconds = 1f;
            [SerializeField] float _afterHalfSpeedUpValue = 0.1f;

            [Header("=== Returning Back To Starting Speed Settings ===")]
            [SerializeField] float _returnBackDuration = 1f;
            [SerializeField] AnimationCurve _returnBackCurve;

            public float SpeedUpPercent => _speedUpPercent;
            public float SpeedUpDuration => _speedUpDuration;
            public AnimationCurve SpeedUpCurve => _speedUpCurve;

            public float AfterHalfSeedpUpEverySeconds => _afterHalfSpeedUpEverySeconds;
            public float AfterHalfSpeedUpValue => _afterHalfSpeedUpValue;

            public float ReturnBackDuration => _returnBackDuration;
            public AnimationCurve RetrunBackCurve => _returnBackCurve;
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
                .SetEase(_moveSettings.SideMoveCurve)
                .OnComplete(() => {_isSwapping = false; });

            _sideMoveTween.OnRewind(() => {_isSwapping = false;});

            _forwardMoveTween.SetAutoKill(false);
            _sideMoveTween.SetAutoKill(false);
        }


        bool _isSwapStarted;
        public void Swap()
        {
            _isSwapStarted = true;
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
            if (!_isSwapStarted) return;

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
            [SerializeField] AnimationCurve _sideMoveCurve;
            [SerializeField] AnimationCurve _forwardMoveCurve;

            public float MoveSideDistance => _moveSideDistance;
            public float MoveForwardDistance => _moveForwardDistance;
            public float MoveDuration => _moveDuration;
            public AnimationCurve SideMoveCurve => _sideMoveCurve;
            public AnimationCurve ForwardMoveCurve => _forwardMoveCurve;
        }
    }
}
