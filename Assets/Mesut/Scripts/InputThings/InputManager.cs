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

        bool _canMouseButtonUp;

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

            if(Input.GetMouseButtonDown(0))
            {
                IsMouseButtonDown = true;
                _canMouseButtonUp = true;
            }
            else
            {
                IsMouseButtonDown = false;
            }

            if (!_canMouseButtonUp)
            {
                IsMouseButtonHold = false;
                IsMouseButtonUp = false;
                return;
            }

            IsMouseButtonHold = Input.GetMouseButton(0);

            if(Input.GetMouseButtonUp(0))
            {
                IsMouseButtonUp = true;
                _canMouseButtonUp = false;
            }
            else
            {
                IsMouseButtonUp = false;
            }
        }

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
        }
    }
}
