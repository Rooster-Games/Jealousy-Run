using System.Collections;
using System.Collections.Generic;
using GameCores;
using GameCores.CoreEvents;
using UnityEngine;

namespace JR
{
    public class CrowdedController : MonoBehaviour
    {
        PersonController[] _personControllerCollection;

        IEventBus _eventBus;
        DollyCartController _dollyCartController;
        public void Init(InitParameters initParameters)
        {
            _eventBus = initParameters.EventBus;
            _dollyCartController = initParameters.DollyCartController;

            _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
            _eventBus.Register<OnBarEmpty>(EventBus_OnBarEmpty);
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _personControllerCollection = GetComponentsInChildren<PersonController>();
            _dollyCartController.StartMoving();
            foreach (var personController in _personControllerCollection)
            {
                personController.Walk();
            }
        }

        private void EventBus_OnBarEmpty(OnBarEmpty eventData)
        {
            _dollyCartController.StopMoving();
        }

        public class InitParameters
        {
            public IEventBus EventBus { get; set; }
            public DollyCartController DollyCartController { get; set; }
        }
    }
}
