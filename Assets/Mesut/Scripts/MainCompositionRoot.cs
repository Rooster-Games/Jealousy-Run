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

            // for initing
            var coreEventBusCompositionRoot = new CoreEventBusCompositionRoot();

            // register for initing
            PMContainer.Instance.RegisterForIniting(_gameManager);
            PMContainer.Instance.RegisterForIniting(coreEventBusCompositionRoot);

            PMContainer.Instance.RegisterForIniting(_playerCompositionRoot);

            _playerCompositionRoot.RegisterToContainer();

            PMContainer.Instance.Solve();
        }
    }
}