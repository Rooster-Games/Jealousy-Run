using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameCores;
using UnityEngine;
using UnityEngine.Rendering;

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

        // SingleController _singleController;

        Gender _protectorGender;
        IProtector _protector;
        //PlayerAnimationEvents _playerAnimationEvents;


        public void Init(InitParameters initParameters)
        {
            _protectorGender = initParameters.ProtectorGender;
            _protector = initParameters.Protector;
            // _playerAnimationEvents = initParameters.PlayerAnimationEvents;
            //_singleController = initParameters.SingleController;

            //_playerAnimationEvents.RgisterOnSlap(Slap);
        }

        private void OnTriggerEnter(Collider other)
        {

            var otherGenderInfo = other.GetComponent<GenderInfo>();
            if(otherGenderInfo == null)
            {
                //Debug.Log(other.gameObject.name);
                //Debug.Log("GenderInfoBulunamadi");
                return;
            }

            if(otherGenderInfo.Gender == _protectorGender)
            //if(otherGenderInfo.Gender == _gender)
            {
                var slapable = other.GetComponent<Slapable>();
                if (slapable == null) return;

                if (_slapableToSlapDataMap.ContainsKey(slapable)) return;
                    //other.enabled = false;

                    var dir = (other.transform.position - transform.position).normalized;
                dir.y = _yDir;
                _forceAmount = Random.Range(_minMaxForceAmount.x, _minMaxForceAmount.y);

                _slapableList.Add(slapable);
                _slapableToSlapDataMap.Add(slapable, (Dir: dir, ForceAmount: _forceAmount));
                float timer = 0f;
                DOTween.To(() => timer, (x) => timer = x, 1f, _slapDelayDuration)
                    .OnComplete(() => slapable.Slap(dir, _forceAmount, _forceMode));

                _protector.Slap();
            }
        }


        Dictionary<Slapable, (Vector3 Dir, float ForceAmount)> _slapableToSlapDataMap = new Dictionary<Slapable, (Vector3 Dir, float ForceAmount)>();
        List<Slapable> _slapableList = new List<Slapable>();

        private void Slap()
        {
            foreach (var slapable in _slapableList)
            {
                var data = _slapableToSlapDataMap[slapable];
                var dir = data.Dir;
                var forceAmount = data.ForceAmount;

                slapable.Slap(dir, forceAmount, _forceMode);
            }

            _slapableList.Clear();
            _slapableToSlapDataMap.Clear();
        }

        public class InitParameters
        {
            // public SingleController SingleController { get; set; }
            public Gender ProtectorGender { get; set; }
            public IProtector Protector { get; set; }
            //public PlayerAnimationEvents PlayerAnimationEvents { get; set; }
        }
    }
}