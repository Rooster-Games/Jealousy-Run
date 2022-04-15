using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCores;
using GameCores.CoreEvents;
using System.Runtime;
using DG.Tweening;

namespace JR
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Transform[] _coupleTransform;

        Settings _settings;
        DollyCartController _dollyCartController;
        PlayerAnimatorController _animatorController;
        EventBus _eventBus;

        bool _swapping;

        public void Init(InitParameters initParameters)
        {
            _dollyCartController = initParameters.DollyCartController;
            _animatorController = initParameters.AnimatorController;
            _settings = initParameters.Settings;
            _eventBus = initParameters.EventBus;

            _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _dollyCartController.StartMoving();
        }

        Tween[] _swapTweens = new Tween[2];
        Vector3[] _moveForwardPoses = new Vector3[2] { new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, -1f) };
        public void SwapPositions()
        {
            _swapping = true;
            var targetPoses = new Vector3[_coupleTransform.Length];
            targetPoses[0] = _coupleTransform[1].localPosition;
            targetPoses[1] = _coupleTransform[0].localPosition;

            for (int i = 0; i < _coupleTransform.Length; i++)
            {
                _swapTweens[i] = _coupleTransform[i].DOLocalMoveX(targetPoses[i].x, _settings.SwapDuration)
                    .OnComplete( () => _swapping = false);
                _coupleTransform[i].DOLocalMoveZ(_moveForwardPoses[i].z, _settings.SwapDuration)
                    .SetEase(_settings.ForwardCurve);
            }
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0) && !_swapping)
            {
                SwapPositions();
            }
        }

        public class InitParameters
        {
            public DollyCartController DollyCartController { get; set; }
            public PlayerAnimatorController AnimatorController { get; set; }
            public Settings Settings { get; set; }
            public EventBus EventBus { get; set; }
        }

        [System.Serializable]
        public class Settings
        {
            [SerializeField] float _swapDuration = 1f;
            [SerializeField] AnimationCurve _forwardCurve;

            public float SwapDuration => _swapDuration;
            public AnimationCurve ForwardCurve => _forwardCurve;
        }
    }
}
