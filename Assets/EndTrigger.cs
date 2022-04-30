using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class EndTrigger : MonoBehaviour
    {
        [SerializeField] InputManager _inputManager;
        bool _isFinished;
        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Other Name: " + other.name);
            var animator = other.GetComponentInChildren<Animator>();
            StartCoroutine(BackToIdle(animator));
            other.GetComponentInParent<SingleController>().ReturnBack();

            _inputManager.IsMouseButtonUp = true;
            _inputManager.enabled = false;

            if (_isFinished) return;
            _isFinished = true;

            RoosterHub.Central.Win();
        }

        IEnumerator BackToIdle(Animator animator)
        {
            Debug.Log("BackToIdle");
            yield return new WaitForSeconds(0.1f);
            _inputManager.ResetMe();
            animator.ResetTrigger("normalRun");
            animator.ResetTrigger("protectRun");
            animator.SetTrigger("idle");
        }
    }
}
