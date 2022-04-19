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

        EventBus _eventBus;
        DollyCartController _dollyCartController;
        public void Init(InitParameters initParameters)
        {
            _personControllerCollection = GetComponentsInChildren<PersonController>();
            _eventBus = initParameters.EventBus;
            _dollyCartController = initParameters.DollyCartController;

            _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _dollyCartController.StartMoving();
            foreach (var personController in _personControllerCollection)
            {
                personController.Walk();
            }
        }

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
            public DollyCartController DollyCartController { get; set; }
        }
    }
}
