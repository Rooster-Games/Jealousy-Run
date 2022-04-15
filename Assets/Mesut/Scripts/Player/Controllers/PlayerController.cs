using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class PlayerController : MonoBehaviour
    {
        DollyCartController _dollyCartController;
        PlayerAnimatorController _animatorController;

        public void Init(InitParameters initParameters)
        {
            _dollyCartController = initParameters.DollyCartController;
            _animatorController = initParameters.AnimatorController;
        }

        public class InitParameters
        {
            public DollyCartController DollyCartController { get; set; }
            public PlayerAnimatorController AnimatorController { get; set; }
        }
    }
}
