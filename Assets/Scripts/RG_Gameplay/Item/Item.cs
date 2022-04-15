using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ICollectable
{
    private float _amount;

    public float Amount { get => _amount; set => _amount = value; }

    public void Execute()
    {
        Debug.LogError("Item executed");
    }
}
