using System.Collections;
using System.Collections.Generic;
using DI;
using GameCores;
using UnityEngine;

namespace JR
{
    public class MainCompositionRoot : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] InputManager _inputManager;
        [SerializeField] PlayerCompositionRoot _playerCompositionRoot;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            // singletons
            var assemblyInstanceCreator = new AssemblyInstanceCreator(typeof(MainCompositionRoot));
            var eventBus = new EventBus();

            // register singletons

            PMContainer.Instance.RegisterSingle(eventBus);
            PMContainer.Instance.RegisterSingle(assemblyInstanceCreator);
            PMContainer.Instance.RegisterSingle(_inputManager);

            // for initing
            var coreEventBusCompositionRoot = new CoreEventBusCompositionRoot();

            // register for initing
            PMContainer.Instance.RegisterForIniting(_gameManager);
            PMContainer.Instance.RegisterForIniting(coreEventBusCompositionRoot);
            PMContainer.Instance.RegisterForIniting(_playerCompositionRoot);
            PMContainer.Instance.RegisterForIniting(_inputManager);

            _playerCompositionRoot.RegisterToContainer();

            PMContainer.Instance.Solve();
        }
    }
}