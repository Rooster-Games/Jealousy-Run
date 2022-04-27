using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class SlapEnemyDetector : MonoBehaviour
    {
        [SerializeField] ForceMode _forceMode;
        [SerializeField] Vector2 _minMaxForceAmount = new Vector2(15f, 35f);
        [SerializeField] float _forceAmount = 50f;
        [SerializeField] float _yDir = 10f;
        [SerializeField] Gender _gender;

        SingleController _singleController;

        public void Init(InitParameters initParameters)
        {
            _singleController = initParameters.SingleController;
        }

        private void OnTriggerEnter(Collider other)
        {
            var otherGenderInfo = other.GetComponent<GenderInfo>();
            if(otherGenderInfo == null)
            {
                Debug.Log(other.gameObject.name);
                Debug.Log("GenderInfoBulunamadi");
                return;
            }

            //if(otherGenderInfo.Gender == _singleController.GenderInfo.Gender)
            if(otherGenderInfo.Gender == _gender)
            {
                var slapable = other.GetComponent<Slapable>();
                if (slapable == null) return;

                var dir = (other.transform.position - transform.position).normalized;
                dir.y = _yDir;
                _forceAmount = Random.Range(_minMaxForceAmount.x, _minMaxForceAmount.y);
                slapable.Slap(dir, _forceAmount, _forceMode);
                

                // if (other.isTrigger)
                   // other.enabled = false;

                // _singleController.Slap();
            }
        }

        public class InitParameters
        {
            public SingleController SingleController { get; set; }
        }
    }
}