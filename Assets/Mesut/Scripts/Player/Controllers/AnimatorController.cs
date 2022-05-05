using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace JR
{
    public class AnimatorController : IAnimatorController
    {
        Animator _animator;
        public void Init(InitParameters initParameters)
        {
            _animator = initParameters.Animator;
        }

        public void SetBool(string stateName, bool state)
        {
            _animator.SetBool(stateName, state);
        }

        public void SetLayerWeight(int layerIndex, float weight)
        {
            _animator.SetLayerWeight(layerIndex, weight);
        }

        public void SetTrigger(string triggerName, [CallerMemberName] string callerName = "")
        {
            _animator.SetTrigger(triggerName);
        }

        public void SetFloat(string parameterName, float value)
        {
            _animator.SetFloat(parameterName, value);
        }

        public void SetAnimatorSpeed(float speed)
        {
            _animator.speed = speed;
        }

        public float TestLength(int layerIndex)
        {
            return _animator.GetCurrentAnimatorStateInfo(layerIndex).length;
        }

        public void ResetTrigger(string stateName)
        {
            _animator.ResetTrigger(stateName);
        }

        public class InitParameters
        {
            public Animator Animator { get; set; }
        }
    }

    public interface IAnimatorController
    {
        void SetBool(string stateName, bool state);
        void SetFloat(string parameterName, float value);
        void SetTrigger(string triggerName, [CallerMemberName] string CallerName = "");
        void SetLayerWeight(int layerIndex, float weight);
        void SetAnimatorSpeed(float speed);
        float TestLength(int layerIndex);
        void ResetTrigger(string stateName);
    }

    public class AnimatorControllerFactory
    {
        public IAnimatorController Create(params Animator[] _animators)
        {
            CompositeAnimatorController _compositeController = new CompositeAnimatorController();
            for(int i = 0; i < _animators.Length; i++)
            {
                var animControllerInitParameters = new AnimatorController.InitParameters();
                animControllerInitParameters.Animator = _animators[i];
                var animController = new AnimatorController();
                animController.Init(animControllerInitParameters);
                if (_animators.Length == 1)
                    return animController;

                _compositeController.Add(animController);
            }

            return _compositeController;
        }
    }

    public class CompositeAnimatorController : IAnimatorController
    {
        List<IAnimatorController> _animationControllerList = new List<IAnimatorController>();

        public void Add(IAnimatorController controller)
        {
            _animationControllerList.Add(controller);
        }

        public void ResetTrigger(string stateName)
        {
            foreach (var controller in _animationControllerList)
            {
                controller.ResetTrigger(stateName);
            }
        }

        public void SetAnimatorSpeed(float speed)
        {
            foreach (var controller in _animationControllerList)
            {
                controller.SetAnimatorSpeed(speed);
            }
        }

        public void SetBool(string stateName, bool state)
        {
            foreach (var controller in _animationControllerList)
            {
                controller.SetBool(stateName, state);
            }
        }

        public void SetFloat(string parameterName, float value)
        {
            foreach (var controller in _animationControllerList)
            {
                controller.SetFloat(parameterName, value);
            }
        }

        public void SetLayerWeight(int layerIndex, float weight)
        {
            foreach (var controller in _animationControllerList)
            {
                controller.SetLayerWeight(layerIndex, weight);
            }
        }

        public void SetTrigger(string triggerName, [CallerMemberName] string callerName = "")
        {
            if (triggerName == "normalRun")
                Debug.Log("CallerName: " + callerName);
            foreach (var controller in _animationControllerList)
            {
                controller.SetTrigger(triggerName);
            }
        }

        public float TestLength(int layerIndex)
        {
            throw new System.NotImplementedException();
        }
    }
}