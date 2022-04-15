using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] private float _animatorSpeed = 1;

    public float AnimatorSpeed { get => _animatorSpeed; set { _animatorSpeed = value; _animator.speed = value; } }

    public bool IsRun { get => _isRunning; set { _isRunning = value; IsRunning(value); } }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        base.Start();
    }
}
