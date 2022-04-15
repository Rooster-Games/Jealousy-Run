using UnityEngine;
using System;
using NaughtyAttributes;


public abstract class CharacterMovement : MonoBehaviour
{
    [Foldout("Info")]
    [ReadOnly]
    [SerializeField] protected bool _isStarted;
    [Foldout("Info")]
    [ReadOnly]
    [SerializeField] protected bool _isFinished;
    [Foldout("Info")]
    [ReadOnly]
    [SerializeField] protected bool _isStopped;

    [Header("Components")]
    [SerializeField] CharacterControl _characterControl;
    [SerializeField] protected Rigidbody _characterRB;
 
    [Header("Movement")]
    [SerializeField] protected float _speed = 5;

    public float Speed { get => _speed; set => _speed = value; }
    public bool IsStopped { get => _isStopped; set => _isStopped = value; }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void StartMovement()
    {
        _isStarted = true;
    }

    protected virtual void StopMovement()
    {
        _isFinished = true;
    }

    protected virtual void Move()
    {
        // Hard Code in Children
    }

    protected virtual void Rotate()
    {
        // Hard Code in Children
    }

    protected virtual void Start()
    {
        _characterRB = GetComponent<Rigidbody>();

    }
}
