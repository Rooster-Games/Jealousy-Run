using System.Collections;
using System.Collections.Generic;
using GameCores;
using GameCores.CoreEvents;
using UnityEngine;

namespace JR
{
    public class InputManager : MonoBehaviour
    {
        public bool IsMouseButtonDown { get; set; }
        public bool IsMouseButtonHold { get; set; }
        public bool IsMouseButtonUp { get; set; }

        bool _isGameStarted;

        EventBus _eventBus;

        public void Init(InitParameters initParameters)
        {
            _eventBus = initParameters.EventBus;

            _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _isGameStarted = true;
        }

        private void Update()
        {
            if (!_isGameStarted) return;

            IsMouseButtonDown = Input.GetMouseButtonDown(0);
            IsMouseButtonHold = Input.GetMouseButton(0);
            IsMouseButtonUp = Input.GetMouseButtonUp(0);

        }

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
        }
    }
}
