using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameCores;
using JR;

public class ItemCollector : MonoBehaviour, ICollector<Item>
{
    IEventBus _eventBus;
    public void Init(InitParameters initParameters)
    {
        _eventBus = initParameters.EventBus;
    }

    public void Collect(Item item)
    {
        item.Execute();
        _eventBus.Fire<OnItemCollected>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Item collectable))
        {
            Collect(collectable);
        }
    }

    public class InitParameters
    {
        public IEventBus EventBus { get; set; }
    }
}
