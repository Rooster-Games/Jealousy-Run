using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JR;
using NaughtyAttributes;
using UnityEngine;

public class TestPush : MonoBehaviour
{
    [SerializeField] float _force;
    [SerializeField] ForceMode _forceMode;
    [SerializeField] Transform _lookAtTransform;
    [SerializeField] float _duration;
    [SerializeField] Vector3 _testDir;

    Transform _myTransform;
    Rigidbody _myBody;
    Animator _anim;
    OtherEnemyDetector _otherEnemyDetector;
    private void Awake()
    {
        _myBody = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _myTransform = transform;
        _otherEnemyDetector = GetComponentInChildren<OtherEnemyDetector>(true);
    }


    [Button("Force Test")]
    public void TestForce()
    {
        var dir = (transform.position - _lookAtTransform.position).normalized;
        dir.y = transform.position.y;
        ForceTest(dir);
    }

    public void ForceTest(Vector3 forceDir)
    {
        var lookPos = _lookAtTransform.position;
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
        _myBody.AddForce(forceDir * _force, _forceMode);

        _anim.SetFloat("hitIndex", Random.Range(0, 3));
        _anim.SetTrigger("hit");
        Debug.Log("Force Test");
    }

    bool _isTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EnemyDetector") return;

        if (_isTriggered) return;

        _isTriggered = true;
        var forceDir = (_myTransform.position - other.transform.position).normalized;
        forceDir.y = _myTransform.position.y;
        _lookAtTransform = other.transform;
        ForceTest(forceDir);

    }
}