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
        [SerializeField] CrowdedControllerCompositionRoot _crowdedControllerCompositionRoot;
        [SerializeField] GameType _gameType;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            var assemblyInstanceCreator = new AssemblyInstanceCreator(typeof(MainCompositionRoot));
            var eventBus = new EventBus();

            // eventBus composition root initing
            var coreEventBusCompositionRoot = new CoreEventBusCompositionRoot();
            var coreEventBusCompositionRootInitParameters = new CoreEventBusCompositionRoot.InitParameters();
            coreEventBusCompositionRootInitParameters.EventBus = eventBus;
            coreEventBusCompositionRootInitParameters.AssemblyInstanceCreator = assemblyInstanceCreator;

            coreEventBusCompositionRoot.Init(coreEventBusCompositionRootInitParameters);

            // input manager Initing
            var inputManagerInitParameters = new InputManager.InitParameters();
            inputManagerInitParameters.EventBus = eventBus;

            _inputManager.Init(inputManagerInitParameters);

            // player composition root init
            var pcrInitParameters = new PlayerCompositionRoot.InitParameters();
            pcrInitParameters.EventBus = eventBus;
            pcrInitParameters.InputManager = _inputManager;
            pcrInitParameters.GameType = _gameType;

            _playerCompositionRoot.Init(pcrInitParameters);

            // crowded composition root init
            var ccrInitparameters = new CrowdedControllerCompositionRoot.InitParameters();
            ccrInitparameters.EventBus = eventBus;
            _crowdedControllerCompositionRoot.Init(ccrInitparameters);


            // game manager init
            var gmInitParameters = new GameManager.InitParameters();
            gmInitParameters.EventBus = eventBus;

            _gameManager.Init(gmInitParameters);
        }
        //public void Init()
        //{
        //    // singletons
        //    var assemblyInstanceCreator = new AssemblyInstanceCreator(typeof(MainCompositionRoot));
        //    var eventBus = new EventBus();

        //    // register singletons

        //    PMContainer.Instance.RegisterSingle(eventBus);
        //    PMContainer.Instance.RegisterSingle(assemblyInstanceCreator);
        //    PMContainer.Instance.RegisterSingle(_inputManager);

        //    // for initing
        //    var coreEventBusCompositionRoot = new CoreEventBusCompositionRoot();

        //    // register for initing
        //    PMContainer.Instance.RegisterForIniting(_gameManager);
        //    PMContainer.Instance.RegisterForIniting(coreEventBusCompositionRoot);
        //    PMContainer.Instance.RegisterForIniting(_playerCompositionRoot);
        //    PMContainer.Instance.RegisterForIniting(_inputManager);

        //    _playerCompositionRoot.RegisterToContainer();
        //    _crowdedControllerCompositionRoot.RegisterToContainer();

        //    PMContainer.Instance.Resolve();
        //}
    }
}