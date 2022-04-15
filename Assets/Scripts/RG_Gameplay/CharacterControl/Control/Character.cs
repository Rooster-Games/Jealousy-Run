using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    [SerializeField] protected bool _isRunning = false;


    protected virtual void OnEnable()
    {
        
    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Start()
    {
        _animator = GetComponent<Animator>();
    }


    protected virtual void IsRunning(bool isRun)
    {
        _isRunning = isRun;

        _animator.SetBool("run",_isRunning);
    }


}
