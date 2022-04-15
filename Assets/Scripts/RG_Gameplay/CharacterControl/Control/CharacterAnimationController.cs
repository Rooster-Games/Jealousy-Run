using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimationController : MonoBehaviour
{
    protected Animator _animator;

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    protected virtual void Start()
    {
        if (_animator == null) _animator = GetComponent<Animator>();

        if (_animator == null) Debug.LogError("Please add Animator");
    }

}
