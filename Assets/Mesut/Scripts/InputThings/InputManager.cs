

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

        IEventBus _eventBus;

        bool _canMouseButtonUp;

        public void Init(InitParameters initParameters)
        {
            _eventBus = initParameters.EventBus;

            _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
            _eventBus.Register<OnBarEmpty>(EventBus_OnBarEmpty);
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _isGameStarted = true;
        }

        private void EventBus_OnBarEmpty(OnBarEmpty eventData)
        {
            _isGameStarted = false;
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

        public void ResetMe()
        {
            IsMouseButtonUp = false;
            IsMouseButtonDown = false;
            IsMouseButtonHold = false;
        }

        public class InitParameters
        {
            public IEventBus EventBus { get; set; }
        }
    }
}
