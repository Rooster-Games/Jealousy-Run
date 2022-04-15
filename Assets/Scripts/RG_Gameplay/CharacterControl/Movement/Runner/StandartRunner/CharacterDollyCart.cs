using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineDollyCart))]
public abstract class CharacterDollyCart : MonoBehaviour
{
    protected CinemachineDollyCart dollyCart;
    [SerializeField] protected float _speed;
    [SerializeField] protected bool _isStopped;

    public float Speed { get => _speed; set => _speed = value; }
    public bool IsStopped { get => _isStopped; set => _isStopped = value; }

    protected virtual void Start()
    {
        dollyCart = GetComponent<CinemachineDollyCart>();
    }

    public virtual void Stop()
    {
        IsStopped = true;
        dollyCart.m_Speed = 0;
    }

    public virtual void Move(float speed)
    {
        Speed = speed;
        IsStopped = false;
        dollyCart.m_Speed = Speed;
    }

    public virtual void SetPath(CinemachinePathBase cinemachinePathBase)
    {
        dollyCart.m_Path = cinemachinePathBase;
    }

    public virtual void SetInitialPosition(Vector3 worldPos)
    {
        float closest = dollyCart.m_Path.FromPathNativeUnits(dollyCart.m_Path.FindClosestPoint(worldPos, 0, -1, 10), CinemachinePathBase.PositionUnits.Distance);
  
        dollyCart.m_Position = closest;
    }
}
