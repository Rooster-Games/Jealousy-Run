using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class StateMachine : MonoBehaviour
    {
        BaseState[] _stateCollection;

        BaseState _currentState;

        public void Init(InitParameters initParameters)
        {
            _stateCollection = initParameters.StateCollection;
        }

        public void Update()
        {
            _currentState.Update();
        }

        public class InitParameters
        {
            public BaseState[] StateCollection { get; set; }
        }
    }

    public class StateTransitions
    {

    }
}