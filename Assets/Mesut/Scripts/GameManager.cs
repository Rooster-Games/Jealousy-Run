using System.Collections;
using System.Collections.Generic;
using DI;
using GameCores;
using GameCores.CoreEvents;
using UnityEngine;

namespace JR
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] bool _developmentMode;

        EventBus _eventbus;

        public void Init(InitParameters initParameters)
        {
            _eventbus = initParameters.EventBus;
        }

        public void Update()
        {
            if (_developmentMode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _developmentMode = false;
                    _eventbus.Fire<OnGameStarted>();
                }
            }
        }

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
        }
    }
}