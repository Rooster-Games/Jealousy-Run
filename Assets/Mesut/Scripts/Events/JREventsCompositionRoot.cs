using System.Collections;
using System.Collections.Generic;
using GameCores;
using UnityEngine;
using System.Linq;

namespace JR
{
    public class JREventsCompositionRoot : IEventBusCompositionRoot
    {
        public void Init(IEventBus eventBus)
        {
            Debug.Log("JREventsCompositionRoot - Init");

            //eventBus.Raise<OnBarEmpty>();
            //eventBus.Raise<OnItemCollected>();
            //eventBus.Raise<OnSlap>();

        }
    }
}