using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class EnemyDetector : MonoBehaviour
    {
        SingleController _singleController;

        public void Init(InitParameters initParameters)
        {
            _singleController = initParameters.SingleController;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("EnemyDetector - OnTriggerEnter");
            var otherGenderInfo = other.GetComponent<GenderInfo>();
            if(otherGenderInfo == null)
            {
                Debug.Log(other.gameObject.name);
                Debug.Log("GenderInfoBulunamadi");
                return;
            }

            Debug.Log("Other Gender: " + otherGenderInfo.Gender);
            Debug.Log("My Gender:" + _singleController.GenderInfo.Gender);
            if(otherGenderInfo.Gender == _singleController.GenderInfo.Gender)
            {
                if (other.isTrigger)
                    other.enabled = false;

                Debug.Log("BeforeSlap");
                _singleController.Slap();
            }
            Debug.Log("Ending Of OnTriggerEnter");
        }

        public class InitParameters
        {
            public SingleController SingleController { get; set; }
        }
    }
}