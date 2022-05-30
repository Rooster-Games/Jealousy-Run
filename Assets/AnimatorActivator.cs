using System.Collections;
using System.Collections.Generic;
using GameCores;
using GameCores.CoreEvents;
using UnityEngine;

namespace JR
{
    public class AnimatorActivator : MonoBehaviour
    {
        BoxCollider _boxCollider;

        public void Init(InitParameters initParameters)
        {
            _boxCollider = initParameters.SelfCollider;

            initParameters.EventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _boxCollider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            var animator = other.GetComponentInChildren<Animator>();

            if (animator != null)
                animator.enabled = true;
        }

        private void OnTriggerExit(Collider other)
        {
            var animator = other.GetComponentInChildren<Animator>();

            if (animator != null)
                animator.enabled = false;
        }

        public class InitParameters
        {
            public IEventBus EventBus { get; set; }
            public BoxCollider SelfCollider { get; set; }
        }
    }
}
