using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
   [SerializeField] protected AIMovement _aIMovement;

    protected virtual void Start()
    {
        _aIMovement = GetComponent<AIMovement>();
    }

    protected virtual void SetTargetPosition(Vector3 targetPos)
    {
        _aIMovement.SetDestination(targetPos);
    }

}
