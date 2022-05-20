
using GameCores;
using UnityEngine;

namespace JR
{
    public class PersonController : MonoBehaviour
    {
        PersonAnimatorController _animatorController;

        IEventBus _eventBus;

        public void Init(InitParameters initParameters)
        {
            _animatorController = GetComponentInChildren<PersonAnimatorController>();
            _eventBus = initParameters.EventBus;
            _eventBus.Register<OnBarEmpty>(EventBus_OnBarEmpty);

        }

        private void EventBus_OnBarEmpty(OnBarEmpty eventData)
        {
            _animatorController.SetTrigger("Idle");
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