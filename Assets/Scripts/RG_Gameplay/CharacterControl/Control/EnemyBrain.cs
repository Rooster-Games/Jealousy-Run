using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : AIBrain
{
    [SerializeField] EnemyCharacter _enemyCharacter;

    [Space]

    [SerializeField] protected Transform _target;

    [Space]
    [Header("States")]
    [SerializeField] protected EnemyState _enemyState;

    [SerializeField] protected bool _isDisabled = false;
    [SerializeField] protected bool _isMoving = false;

    [Space]
    [Header("StateSpeeds")]
    [SerializeField] protected float _patrolSpeed = 1;
    [SerializeField] protected float _offensiveSpeed = 3;

    protected enum EnemyState
    {
        Idle,
        Patrol,
        Offensive,
        Disable
    }

    protected virtual void OnEnable()
    {
        _aIMovement.OnMove += IsMoving;
        _aIMovement.OnReachDestination += OnEnemyStopMoving;
        GetComponentInChildren<TargetDetector>().OnTargetSeen += Alarmed;
        GetComponentInChildren<TargetDetector>().OnTargetGone += CancelAlarmed;
    }

    protected virtual void OnDisable()
    {
        _aIMovement.OnMove -= IsMoving;
        _aIMovement.OnReachDestination -= OnEnemyStopMoving;
        GetComponentInChildren<TargetDetector>().OnTargetSeen -= Alarmed;
        GetComponentInChildren<TargetDetector>().OnTargetGone -= CancelAlarmed;
    }

    protected override void Start()
    {
        base.Start();

        if (!_enemyCharacter) _enemyCharacter = GetComponent<EnemyCharacter>();
    }

    protected override void SetTargetPosition(Vector3 targetPos)
    {
        base.SetTargetPosition(targetPos);

        _enemyCharacter.IsRun = _isMoving;
    }

    protected virtual void Alarmed(Transform _target)
    {
        if (!_isDisabled)
        {
            SetTarget(_target);
            BeOffensive();
        }
    }

    protected virtual void CancelAlarmed(Transform _)
    {
        _target = null;
        BeNormal();
    }

    protected virtual void SetTarget(Transform target)
    {
        _target = target;
    }

    protected virtual void SetEnemyState(EnemyState enemyState)
    {
        _enemyState = enemyState;
    }

    protected virtual void BeOffensive()
    {
        SetEnemyState(EnemyState.Offensive);
        _aIMovement.SetSpeed(_offensiveSpeed);
        SetTargetPosition(_target.position);
        _enemyCharacter.AnimatorSpeed = _offensiveSpeed * (_patrolSpeed / _offensiveSpeed) * 0.75f;
    }

    protected virtual void BeDisable()
    {
        _isDisabled = true;
        SetEnemyState(EnemyState.Disable);
    }

    protected virtual void BeNormal()
    {
        SetEnemyState(EnemyState.Idle);
        _aIMovement.SetSpeed(_patrolSpeed);
        _enemyCharacter.AnimatorSpeed = 1;

    }

    protected virtual void IsMoving(bool isMove)
    {
        _isMoving = isMove;
    }

    protected virtual void OnEnemyStopMoving()
    {
        if (_target)
        {
            //Debug.LogError("Attack");

            _isMoving = false;

            _enemyCharacter.IsRun = _isMoving;
        }
        else
        {
            BeNormal();
            //Debug.LogError("Idle");

            _isMoving = false;

            _enemyCharacter.IsRun = _isMoving;
        }
    }

}
