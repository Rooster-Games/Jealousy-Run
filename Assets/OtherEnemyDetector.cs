using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class OtherEnemyDetector : MonoBehaviour
    {
        ForceMode _forceMode;
        float _forceAmount;

        public void Init(InitParameters initParameters)
        {
            _forceMode = initParameters.ForceMode;
            _forceAmount = initParameters.ForceAmount;
        }

        private void OnTriggerEnter(Collider other)
        {
            var pushable = other.GetComponent<Pushable>();
            if (pushable == null) return;

            var dir = (other.transform.position - transform.position).normalized;
            pushable.Push(dir, _forceAmount, _forceMode);
        }

        public class InitParameters
        {
            public ForceMode ForceMode { get; set; }
            public float ForceAmount { get; set; }
        }
    }
}