using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(InputManager))]
public class StandartRunnerMovement : CharacterMovement
{
    InputManager inputManager;

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

        inputManager = GetComponent<InputManager>();

        if (inputManager == null) Debug.LogError("Please add InputManager");
    }

    protected override void Move()
    {
        if (_isStarted && !IsStopped)
        {
            float sideValue = Mathf.Clamp(Mathf.Lerp(0, _sideLimit * Mathf.Sign(inputManager.SmoothDelta.x), Mathf.Abs(inputManager.SmoothDelta.x)), -_sideLimit, _sideLimit);
            Vector3 targetPos = transform.position + Vector3.forward * Speed * Time.deltaTime;
            transform.localPosition = new Vector3(sideValue, 0, targetPos.z);
            OnRun?.Invoke(true);
        }
    }

    protected override void Rotate()
    {
        
    }

    private void Update()
    {
        Move();
    }
}
