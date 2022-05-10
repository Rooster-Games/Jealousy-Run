using System.Collections;
using System.Collections.Generic;
using DIC;
using GameCores;
using UnityEngine;

namespace DITEST
{
    public class MainCompositionRoot : MonoBehaviour
    {
        [SerializeField] TestControllerCompositionRoot _testControllerCompositionRoot;


        private void Awake()
        {
            RegisterToContainer();
        }

        private void RegisterToContainer()
        {
            var assemblyCreator = new AssemblyInstanceCreator(typeof(MainCompositionRoot));
            //var eventBus = new IEventBus();
            var coreEventBusCompositionRoot = new CoreEventBusCompositionRoot();

            //DIContainer.Instance.RegisterSingle(eventBus);
            DIContainer.Instance.RegisterSingle(assemblyCreator);
            DIContainer.Instance.RegisterSingle(coreEventBusCompositionRoot);

            _testControllerCompositionRoot.RegisterToContainer();

            DIContainer.Instance.Resolve();
        }
    }
}
