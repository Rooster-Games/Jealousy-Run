using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace JR
{
    public class Slapable : MonoBehaviour
    {
        [SerializeField] Rigidbody _myBody;
        [SerializeField] Vector3 _torqueDir;
        [SerializeField] float _otherDetectorOpenAfterSeconds = 0.25f;

        [SerializeField] GameObject _closeObject;
        [SerializeField] GameObject _ragdoll;
        [SerializeField] OtherEnemyDetectorToSlap _slapDetector;
        Animator _anim;

        DynamicBone _dynamicBone;
        BoxCollider _boxCollider;

        [SerializeField]Rigidbody[] _ragdollBodies;

        float maxMass = 24f;

        private void Awake()
        {
            _anim = GetComponentInChildren<Animator>();
            //_slapDetector = GetComponentInChildren<OtherEnemyDetectorToSlap>(true);
            _boxCollider = GetComponent<BoxCollider>();
            _dynamicBone = GetComponentInChildren<DynamicBone>();
        }

        bool _isSlapped;

        public void Slap(Vector3 dir, float forceAmount, ForceMode forceMode)
        {
            if (_isSlapped) return;
            _isSlapped = true;
            _closeObject.SetActive(false);
            _ragdoll.SetActive(true);
            _boxCollider.isTrigger = false;
            _myBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            var oedSlapInitParameter = new OtherEnemyDetectorToSlap.InitParameters();
            oedSlapInitParameter.ForceAmount = forceAmount;
            oedSlapInitParameter.ForceMode = forceMode;

            _slapDetector.Init(oedSlapInitParameter);

            _myBody.AddForce(dir * forceAmount, forceMode);
            _myBody.AddTorque(dir * forceAmount, forceMode);

            StartCoroutine(AddFoceCO(dir, forceAmount, forceMode));

            float timer = 0f;
            DOTween.To(() => timer, (x) => timer = x, 1f, _otherDetectorOpenAfterSeconds)
                .OnComplete(() => _slapDetector.gameObject.SetActive(true));
            _myBody.useGravity = true;
            _dynamicBone.m_Stiffness = 0.15f;
        }

        IEnumerator AddFoceCO(Vector3 dir, float forceAmount, ForceMode forceMode)
        {
            for (int i = 0; i < 2; i++)
            {
                AddForce(dir, forceAmount, forceMode);
                yield return null;
            }
        }

        private void AddForce(Vector3 dir, float forceAmount, ForceMode forceMode)
        {
            foreach (var rb in _ragdollBodies)
            {
                rb.AddForce(dir * forceAmount * (rb.mass / maxMass), forceMode);
                //rb.AddTorque(dir * forceAmount, forceMode);
            }
        }
    }
}