
using System.Collections;
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

        IEventBus _eventbus;

        bool _isInitialized;

        public void Init(InitParameters initParameters)
        {
            _eventbus = initParameters.EventBus;
            _isInitialized = true;

            RoosterHub.Central.OnGameStartedHandler += StartGame;
            _eventbus.Register<OnBarEmpty>(EventBus_OnBarEmpty);
            _eventbus.Register<OnGameWin>(EventBus_OnGameWin);
        }

        private void EventBus_OnBarEmpty(OnBarEmpty eventData)
        {
            StartCoroutine(Fail());
        }

        IEnumerator Fail()
        {
            yield return new WaitForSeconds(1f);
            RoosterHub.Central.Fail();
        }


        private void EventBus_OnGameWin(OnGameWin eventData)
        {
            RoosterHub.Central.Win();
        }

        private void StartGame()
        {
            _isGameStarted = true;
            _eventbus.Fire<OnGameStarted>();
        }

        public void Update()
        {
            if (!_isInitialized) return;

            if (Input.GetKeyDown(KeyCode.P))
                _developmentMode = !_developmentMode;

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
            // PMContainer.Reset();
            RoosterHub.Central.OnGameStartedHandler -= StartGame;
        }

        public class InitParameters
        {
            public IEventBus EventBus { get; set; }
        }
    }
}