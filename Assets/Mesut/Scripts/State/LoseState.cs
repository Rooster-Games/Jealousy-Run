using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class LoseState : BaseState
    {
        public void Init(InitParameters initParameters)
        {
            base.Init(initParameters);
        }

        public new class InitParameters : BaseState.InitParameters
        {

        }
    }
}