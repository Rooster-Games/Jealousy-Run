using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class OtherEnemyDetectorToSlap : MonoBehaviour
    {
        [SerializeField] float _forcePercent = 0.75f;
        ForceMode _forceMode;
        float _forceAmount;

        public void Init(InitParameters initParameters)
        {
            _forceMode = initParameters.ForceMode;
            _forceAmount = initParameters.ForceAmount * _forcePercent;
        }

        private void OnTriggerEnter(Collider other)
        {
            var slapable = other.GetComponent<Slapable>();
            if (slapable == null) return;

            var dir = (other.transform.position - transform.position).normalized;

            slapable.Slap(dir, _forceAmount, _forceMode);
        }

        public class InitParameters
        {
            public ForceMode ForceMode { get; set; }
            public float ForceAmount { get; set; }
        }
    }
}