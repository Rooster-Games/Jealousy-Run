using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class AIMovement : CharacterMovement
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public event Action OnReachDestination;

    public bool IsMoving = false;

    public event Action<bool> OnMove;

    protected new virtual void OnEnable()
    {
        base.OnEnable();
    }

    protected new virtual void OnDisable()
    {
        base.OnDisable();
    }

    protected new virtual void Start()
    {
        base.Start();

        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public virtual void SetDestination(Vector3 targetPos)
    {
        _navMeshAgent.SetDestination(targetPos);
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
        _navMeshAgent.speed = Speed;
    }

    public virtual float GetRemainingDistance()
    {
        return _navMeshAgent.remainingDistance;
    }

    public virtual void Stop()
    {
        _navMeshAgent.isStopped = true;
    }
    public virtual void KeepGoing()
    {
        _navMeshAgent.isStopped = false;
        
    }

    void CheckAgentConditions()
    {
        if (!_navMeshAgent.isStopped)
        {
            Debug.Log(_navMeshAgent.velocity.magnitude);

            if(_navMeshAgent.velocity.magnitude > 0)
            {
                if(_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
                {
                    OnReachDestination?.Invoke();
                    IsMoving = false;
                    //Debug.LogError("Reached");
                }
                else
                {
                    IsMoving = true;
                }
            }
            else
            {
                IsMoving = false;
            }

            OnMove?.Invoke(IsMoving);
        }
    }

    private void Update()
    {
        CheckAgentConditions();
    }
}
