using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace JR
{
    public class Pushable : MonoBehaviour
    {
        Rigidbody _myBody;
        Animator _anim;
        [SerializeField] float _otherDetectorOpenAfterSeconds = 0.25f;
        [SerializeField] OtherEnemyDetector _otherEnemyDetector;

        bool _isPushed;

        private void Awake()
        {
            _myBody = GetComponent<Rigidbody>();
            _anim = GetComponentInChildren<Animator>();
            _otherEnemyDetector = GetComponentInChildren<OtherEnemyDetector>(true);
        }

        public void Push(Vector3 direction, float force, ForceMode forceMode)
        {
            if (_isPushed) return;

            var oedInitParameters = new OtherEnemyDetector.InitParameters();
            oedInitParameters.ForceAmount = force;
            oedInitParameters.ForceMode = forceMode;
            _otherEnemyDetector.Init(oedInitParameters);

            _isPushed = true;
            _myBody.AddForce(direction * force, forceMode);

            _anim.SetFloat("hitIndex", Random.Range(0, 4));
            _anim.SetTrigger("hit");

            float timer = 0f;
            DOTween.To(() => timer, (x) => timer = x, 1f, _otherDetectorOpenAfterSeconds)
                .OnComplete(() => _otherEnemyDetector.gameObject.SetActive(true));
        }
    }
}