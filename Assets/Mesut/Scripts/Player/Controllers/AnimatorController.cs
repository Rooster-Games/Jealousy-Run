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

        public void SetTrigger(string triggerName)
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

        public float GetFloat(string parameterName)
        {
            return _animator.GetFloat(parameterName);
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
        float GetFloat(string parameterName);
        void SetTrigger(string triggerName);
        void SetLayerWeight(int layerIndex, float weight);
        void SetAnimatorSpeed(float speed);
        float TestLength(int layerIndex);
        void ResetTrigger(string stateName);
    }

    public class AnimatorControllerFactory
    {
        public IAnimatorController Create(FactoryCreateParameters createParameters)
        {
            var animControllerInitParameters = new AnimatorController.InitParameters();

            //Debug.Log("Is Animator Null: " + (createParameters.Animator == null));
            // Debug.Log("Is RunTimeAnimatorcontroller Null: " + (createParameters.RuntimeAnimatorController == null));

            animControllerInitParameters.Animator = createParameters.Animator;
            // createParameters.Animator.runtimeAnimatorController = createParameters.RuntimeAnimatorController;

            var animController = new AnimatorController();
            animController.Init(animControllerInitParameters);

            return animController;
        }

        public class FactoryCreateParameters
        {
            public Animator Animator { get; set; }
            // public RuntimeAnimatorController RuntimeAnimatorController { get; set; }
        }
    }

    public class CompositeAnimatorController : IAnimatorController
    {
        List<IAnimatorController> _animationControllerList = new List<IAnimatorController>();

        public void Add(IAnimatorController controller)
        {
            _animationControllerList.Add(controller);
        }

        public float GetFloat(string parameterName)
        {
            throw new System.NotImplementedException();
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

        public void SetTrigger(string triggerName)
        {
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