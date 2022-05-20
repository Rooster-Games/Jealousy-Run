using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCores
{
    namespace CoreEvents
    {
        public struct OnGameStarted : IEventData { }
        public struct OnGameWin : IEventData { }
        public struct OnGameFail : IEventData { }
    }
}