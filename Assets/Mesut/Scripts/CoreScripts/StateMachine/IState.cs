using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCores
{
    public interface IState
    {
        void OnEnter();
        void Update();
        void OnExit();
    }
}