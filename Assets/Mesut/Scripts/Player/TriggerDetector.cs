using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class TriggerDetector : MonoBehaviour
    {
        SingleController _singleController;

        public void Init(InitParameters initParameters)
        {
            _singleController = initParameters.SingleController;
        }

        private void OnTriggerEnter(Collider other)
        {
            var otherGenderInfo = other.GetComponent<GenderInfo>();
            if(otherGenderInfo.Gender == _singleController.GenderInfo.Gender)
            {
                if (other.isTrigger)
                    other.enabled = false;

                _singleController.Slap();
            }
        }

        public class InitParameters
        {
            public SingleController SingleController { get; set; }
        }
    }
}