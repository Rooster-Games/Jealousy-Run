
using System.Collections;
using DG.Tweening;
using GameCores;
using UnityEngine;

namespace JR
{
    public class PersonController : MonoBehaviour
    {
        public bool IsInsideTheBox { get; set; }
        PersonAnimatorController _animatorController;

        IEventBus _eventBus;

        public void Init(InitParameters initParameters)
        {
            _animatorController = GetComponentInChildren<PersonAnimatorController>();
            _eventBus = initParameters.EventBus;
            _eventBus.Register<OnBarEmpty>(EventBus_OnBarEmpty);
            _animatorController.SetTrigger("Idle");
            StartCoroutine(DisableAnimator());
        }

        IEnumerator DisableAnimator()
        {
            yield return new WaitForSeconds(0.5f);
            _animatorController.EnableAnimator(false);
        }


        private void EventBus_OnBarEmpty(OnBarEmpty eventData)
        {
            if(!Mathf.Approximately(transform.localEulerAngles.y, 0f))
            {
                transform.DOLocalRotate(Vector3.up * 180f, 0.25f);
            }

            // _animatorController.SetTrigger("idle");
        }

        public void Walk()
        {
            _animatorController.SetFloat("walkIndex", Random.Range(0, 7));
            _animatorController.SetTrigger("walk");
        }

        public class InitParameters
        {
            public IEventBus EventBus { get; set; }
        }
    }
}