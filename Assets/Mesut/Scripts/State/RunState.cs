using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class RunState : BaseState
    {
        public void Init(InitParameters initParameters)
        {
            base.Init(initParameters);
        }

        public override void OnEnter()
        {
            _animatorController.SetTrigger("normalRun");
        }

        public new class InitParameters: BaseState.InitParameters
        {

        }
    }
}