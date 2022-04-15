using System;
using UnityEngine;

public interface ICollectable
{
    public float Amount { get; set; }

    public void Execute();
}

public interface ICollector<T> where T : ICollectable
{
    public void Collect(T collectable);
}
