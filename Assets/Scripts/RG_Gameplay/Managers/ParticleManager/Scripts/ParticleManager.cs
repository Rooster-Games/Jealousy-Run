using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(OldParticlePool))]
public class ParticleManager : Singleton<ParticleManager>
{
    OldParticlePool particlePool;

    public override void Awake()
    {
        base.Awake();
        particlePool = GetComponent<OldParticlePool>();
    }

   public void PlayParticle(string name,Vector3 worldPosition,Quaternion rotation)
    {
        if(particlePool.SearchPool(name,out ParticleSystem particle))
        {
            particle.transform.position = worldPosition;
            particle.transform.rotation = rotation;
            particle.Play();
        }
    }

}


