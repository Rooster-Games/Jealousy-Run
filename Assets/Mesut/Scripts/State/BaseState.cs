using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCores;

namespace JR
{
    public abstract class BaseState : IState
    {
        protected IAnimatorController _animatorController;

        public void Init(InitParameters initParameters)
        {
            _animatorController = initParameters.AnimatorController;
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnExit()
        {

        }

        public virtual void Update()
        {
        }

        public class InitParameters
        {
            public IAnimatorController AnimatorController { get; set; }
        }
    }
}
