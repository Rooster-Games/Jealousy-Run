using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        [SerializeField] float _slapDelayDuration = 0.33f;
        [SerializeField] BarController _barController;
        [SerializeField] float _barChangeAmount = 0.1f;

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

            if(otherGenderInfo.Gender == _singleController.GenderInfo.Gender)
            //if(otherGenderInfo.Gender == _gender)
            {
                var slapable = other.GetComponent<Slapable>();
                if (slapable == null) return;

                var dir = (other.transform.position - transform.position).normalized;
                dir.y = _yDir;
                _forceAmount = Random.Range(_minMaxForceAmount.x, _minMaxForceAmount.y);

                float timer = 0f;
                DOTween.To(() => timer, (x) => timer = x, 1f, _slapDelayDuration)
                    .OnComplete(() => slapable.Slap(dir, _forceAmount, _forceMode));

                _singleController.Slap(() => SlapAction(slapable, dir));
            }
        }

        private void SlapAction(Slapable slapable, Vector3 dir)
        {
            _barController.ChangeAmount(_barChangeAmount);
            //float timer = 0f;
            //DOTween.To(() => timer, (x) => timer = x, 1f, _slapDelayDuration)
            //    .OnComplete(() => slapable.Slap(dir, _forceAmount, _forceMode));
        }


        public class InitParameters
        {
            public SingleController SingleController { get; set; }
        }
    }
}