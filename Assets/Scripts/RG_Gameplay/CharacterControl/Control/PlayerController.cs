using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : CharacterControl
{

    public event Action OnStart;
    public event Action OnFinish;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnStart?.Invoke();
        }
    }
}
