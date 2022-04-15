using System.Collections;
using System.Collections.Generic;
using DI;
using UnityEngine;

namespace GameCores
{
    public class CoreCompositionRoot
    {
        public void Init(InitParameters initParameters)
        {
            var eventBus = initParameters.EventBus;
            var assemlyInstanceCreator = initParameters.AssemblyInstanceCreator;

            CoreEventBusCompositionRoot eventBusCompositionRoot = new CoreEventBusCompositionRoot();
            var eventBusCompositionRootInitPameters = new CoreEventBusCompositionRoot.InitParameters();
            eventBusCompositionRootInitPameters.EventBus = eventBus;
            eventBusCompositionRootInitPameters.AssemblyInstanceCreator = assemlyInstanceCreator;
            eventBusCompositionRoot.Init(eventBusCompositionRootInitPameters);
        }

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
            public AssemblyInstanceCreator AssemblyInstanceCreator { get; set; }
        }
    }
}
