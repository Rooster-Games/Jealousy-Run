using UnityEngine;
using System;
using NaughtyAttributes;

public abstract class CharacterControl : MonoBehaviour
{
    [Foldout("Info")] [ReadOnly] [SerializeField] protected bool _isStarted;
    [Foldout("Info")] [ReadOnly] [SerializeField] protected bool _isFinished;

    public bool IsStarted { get => _isStarted; set => _isStarted = value; }
    public bool IsFinished { get => _isFinished; set => _isFinished = value; }



    protected virtual void StartRace()
    {
        IsStarted = true;

    }

    protected virtual void FinishRace()
    {
        IsFinished = true;

    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Start()
    {

    }

}
