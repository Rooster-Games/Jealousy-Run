using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class DefendState : BaseState
    {
        public void Init(InitParameters initParameters)
        {
            base.Init(initParameters);
        }

        public override void OnEnter()
        {
            _animatorController.SetTrigger("protectRun");
        }

        public new class InitParameters : BaseState.InitParameters
        {

        }
    }
}