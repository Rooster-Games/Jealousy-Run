using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemCollector : MonoBehaviour, ICollector<Item>
{
    public event Action<Item> OnItemCollected;

    public void Collect(Item item)
    {
        item.Execute();
        OnItemCollected?.Invoke(item);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Item collectable))
        {
            Collect(collectable);
        }
    }
}
