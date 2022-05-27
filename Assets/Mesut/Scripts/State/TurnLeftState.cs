using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class TurnLeftState : BaseState
    {
        public void Init(InitParameters initParameters)
        {
            base.Init(initParameters);
        }

        public override void OnEnter()
        {
            _animatorController.SetTrigger("turnLeft");
        }

        public override void OnExit()
        {
        }

        public new class InitParameters : BaseState.InitParameters
        {

        }
    }
}