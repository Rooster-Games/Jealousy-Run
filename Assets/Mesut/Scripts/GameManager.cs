using System.Collections;
using System.Collections.Generic;
using DI;
using GameCores;
using GameCores.CoreEvents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JR
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] bool _developmentMode;
        bool _isGameStarted;

        EventBus _eventbus;

        public void Init(InitParameters initParameters)
        {
            _eventbus = initParameters.EventBus;
        }

        public void Update()
        {
            if (_developmentMode)
            {
                if (Input.GetMouseButtonDown(0) && !_isGameStarted)
                {
                    _isGameStarted = true;
                    _eventbus.Fire<OnGameStarted>();
                }

                if(Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }

        private void OnDisable()
        {
            PMContainer.Reset();
        }

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
        }
    }
}