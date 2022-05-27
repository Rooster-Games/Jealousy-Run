using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DIC;
using GameCores;
using GameCores.CoreEvents;
using UnityEngine;

namespace JR
{
    public class EndTrigger : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera _endCamera;
        InputManager _inputManager;
        IEventBus _eventBus;

        bool _isFinished;

        public void Init(InitParameters initParameters)
        {
            _inputManager = initParameters.InputManager;
            _eventBus = initParameters.EventBus;
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Other Name: " + other.name);
            var animator = other.GetComponentInChildren<Animator>();
            StartCoroutine(OnEnd(animator));
            other.GetComponentInParent<SingleController>().ReturnBack();

            _inputManager.IsMouseButtonUp = true;
            _inputManager.enabled = false;

            if (_isFinished) return;
            _isFinished = true;

            _endCamera.Priority = 11;
            _eventBus.Fire<OnGameWin>();
        }

        IEnumerator OnEnd(Animator animator)
        {
            Debug.Log("BackToIdle");
            yield return new WaitForSeconds(0.1f);
            _inputManager.ResetMe();
            //animator.ResetTrigger("normalRun");
            //animator.ResetTrigger("protectRun");
            //animator.ResetTrigger("yorgun");
            //animator.ResetTrigger("panicRun");
            //animator.ResetTrigger("turnLeft");
            //animator.ResetTrigger("truenRight");
            //animator.ResetTrigger("protectRun");
            //animator.SetTrigger("idle");
            animator.SetTrigger("onEnd");
        }


        public class InitParameters
        {
            public InputManager InputManager { get; set; }
            public IEventBus EventBus { get; set; }
        }
    }
}
