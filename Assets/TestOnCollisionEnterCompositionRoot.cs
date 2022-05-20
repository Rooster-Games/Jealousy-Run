using System.Collections;
using System.Collections.Generic;
using JR;
using RootMotion.Dynamics;
using UnityEngine;

public class TestOnCollisionEnterCompositionRoot : MonoBehaviour
{
    [SerializeField] float _forceAmount;
    Rigidbody[] _myBodies;

    PuppetMaster _myPuppet;

    private void Awake()
    {
        _myBodies = GetComponentsInChildren<Rigidbody>();
        _myPuppet = GetComponent<PuppetMaster>();
        StartCoroutine(Test());
        foreach (var oncollision in GetComponentsInChildren<TestOnCollisionEnter>())
        {
            oncollision.Init(this);
        }
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(0.25f);
        _myPuppet.pinWeight = 0.85f;
        _myPuppet.muscleWeight = 0.85f;
    }

    public void AddForce(Vector3 direction)
    {
        for (int i = 0; i < _myBodies.Length; i++)
            _myBodies[i].AddForce(direction * _forceAmount);
    }
}
