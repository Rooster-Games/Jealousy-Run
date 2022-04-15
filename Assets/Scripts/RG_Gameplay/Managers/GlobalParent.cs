using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalParent : Singleton<GlobalParent>
{
    [SerializeField] private Transform _particleParent;

    [SerializeField] private Transform _triggerParent;

    [SerializeField] private Transform _garbageParent;

    public Transform ParticleParent
    {
        get
        {
            if (_particleParent) return _particleParent;
            else return transform;
        }
    }

    public Transform TriggerParent
    {
        get
        {
            if (_triggerParent) return _triggerParent;
            else return transform;
        }
    }

    public Transform GarbageParent
    {
        get
        {
            if (_garbageParent) return _garbageParent;
            else return transform;
        }
    }



    public override void Awake()
    {
        base.Awake();
    }
}
