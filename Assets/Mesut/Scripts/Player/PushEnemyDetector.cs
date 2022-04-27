using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class PushEnemyDetector : MonoBehaviour
    {
        [SerializeField] ForceMode _forceMode;
        [SerializeField] float _forceAmount = 2500f;

        private void OnTriggerEnter(Collider other)
        {
            var pushable = other.GetComponent<Pushable>();
            if (pushable == null) return;

            var otherPos = other.transform.position;
            otherPos.y = 0f;
            var myPos = transform.position;
            myPos.y = 0f;

            var dir = (otherPos - myPos).normalized;
            pushable.Push(dir, _forceAmount, _forceMode);
        }
    }
}