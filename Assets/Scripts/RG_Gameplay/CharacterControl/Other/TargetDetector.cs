using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetDetector : MonoBehaviour
{
    [SerializeField] protected Transform _target;

    [SerializeField] protected LayerMask _checkLayer;

    [SerializeField] protected float _checkDistance;

    public event Action<Transform> OnTargetClose;
    public event Action<Transform> OnTargetSeen;
    public event Action<Transform> OnTargetGone;

    Ray ray;
    RaycastHit hit;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerController playerController))
        {
            _target = playerController.transform;
            OnTargetClose?.Invoke(_target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            OnTargetGone?.Invoke(_target);
            _target = null;
        }
    }

    private void Update()
    {
        if (_target)
        {
            ray = new Ray(transform.position,_target.position - transform.position);

            Debug.DrawRay(ray.origin, ray.direction * _checkDistance,Color.blue);

            if (Physics.Raycast(ray,out hit,_checkDistance,_checkLayer))
            {
                if(hit.collider.gameObject.TryGetComponent(out PlayerController playerController))
                {
                    OnTargetSeen?.Invoke(hit.transform);
                }
                else
                {
                    OnTargetGone?.Invoke(_target);
                }
            
            }
            else
            {
                OnTargetGone?.Invoke(_target);
            }
        }
    }
}
