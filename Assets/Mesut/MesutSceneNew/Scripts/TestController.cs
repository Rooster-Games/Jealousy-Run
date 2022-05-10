using System.Collections;
using System.Collections.Generic;
using GameCores;
using GameCores.CoreEvents;
using JR;
using UnityEngine;

public class TestController : MonoBehaviour
{
    IAnimatorController _animatorController;
    IEventBus _eventBus;

    public void Init(InitParameters initParameters)
    {
        Debug.Log("Init");

        _animatorController = initParameters.AnimatorController;
        _eventBus = initParameters.EventBus;

        Debug.Log("Is Animator Controller Null: " + (_animatorController == null));
        Debug.Log("Is EventBus Null: " + (_eventBus == null));

        _eventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
    }

    public void EventBus_OnGameStarted(OnGameStarted eventData)
    {
        Debug.Log("Test Controller - OnGameStarted");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Is Animator Controller Null: " + (_animatorController == null));
        }
    }

    public class InitParameters
    {
        public IAnimatorController AnimatorController { get; set; }
        public IEventBus EventBus { get; set; }
    }
}
