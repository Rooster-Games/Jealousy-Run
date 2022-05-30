using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCores;
using GameCores.CoreEvents;

public class ItemActivator : MonoBehaviour
{
    BoxCollider _boxCollider;

    public void Init(InitParameters initParameters)
    {
        _boxCollider = initParameters.SelfCollider;

        initParameters.EventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
    }

    private void EventBus_OnGameStarted(OnGameStarted eventData)
    {
        _boxCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.SetActive(false);
    }

    public class InitParameters
    {
        public IEventBus EventBus { get; set; }
        public BoxCollider SelfCollider { get; set; }
    }
}
