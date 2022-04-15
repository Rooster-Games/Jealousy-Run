using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(InputManager))]
public class PathRunnerMovement : CharacterMovement
{
    InputManager inputManager;

    PlayerDollyCart playerDollyCart;

    [SerializeField] private float _sideLimit = 3.5f;

    public event Action<bool> OnRun;

    protected override void OnEnable()
    {
        base.OnEnable();

        GetComponent<PlayerController>().OnStart += StartMovement;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        GetComponent<PlayerController>().OnStart -= StartMovement;
    }

    protected override void Start()
    {
        base.Start();

        playerDollyCart = GetComponentInParent<PlayerDollyCart>();

        if (playerDollyCart == null) Debug.LogError("Please add PlayerDollyCart");

        inputManager = GetComponent<InputManager>();

        if (inputManager == null) Debug.LogError("Please add InputManager");
    }

    protected override void StartMovement()
    {
        base.StartMovement();
        playerDollyCart.Move(Speed);
    }

    protected override void StopMovement()
    {
        base.StopMovement();
    }

    protected override void Move()
    {
        if (_isStarted && !IsStopped)
        {
            float sideValue = Mathf.Clamp(Mathf.Lerp(0, _sideLimit * Mathf.Sign(inputManager.SmoothDelta.x), Mathf.Abs(inputManager.SmoothDelta.x)), -_sideLimit, _sideLimit);
            transform.localPosition = new Vector3(sideValue, 0, 0);
            OnRun?.Invoke(true);
        }
    }

    private void Update()
    {
        Move();
    }
}
