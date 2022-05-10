using System.Collections;
using System.Collections.Generic;
using GameCores;
using UnityEngine;

namespace JR
{
    public class JREventsCompositionRoot : IEventBusCompositionRoot
    {
        public void Init(IEventBus eventBus)
        {
            Debug.Log("JREventsCompositionRoot - Init");

            eventBus.Raise<OnBarEmpty>();
            eventBus.Raise<OnLevelEnd>();
        }
    }
}