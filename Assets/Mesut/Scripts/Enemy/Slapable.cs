using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;

namespace JR
{
    public class Slapable : MonoBehaviour
    {
        [SerializeField] Rigidbody _myBody;
        [SerializeField] Vector3 _torqueDir;
        [SerializeField] float _otherDetectorOpenAfterSeconds = 0.25f;

        public GameObject _closeObject;
        public GameObject _ragdoll;
        [SerializeField] OtherEnemyDetectorToSlap _slapDetector;
        Animator _anim;

        DynamicBone _dynamicBone;
        BoxCollider _boxCollider;

        public Rigidbody[] _ragdollBodies;
        [field: SerializeField] public bool CanSlap { get; set; } = true;

        float maxMass = 24f;

        ParentSettings _parentSettings;

        public void Init(InitParameters initParameters)
        {
            _parentSettings = initParameters.ParentSettings;
        }

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
            //_closeObject.GetComponent<DynamicBone>().enabled = false;
            _closeObject.SetActive(false);
            _ragdoll.SetActive(true);

            float timer = 0f;
            DOTween.To(() => timer, (x) => timer = x, 1f, 1f)
                .OnComplete(() => _ragdoll.SetActive(false));

            _boxCollider.isTrigger = false;
            _myBody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            var oedSlapInitParameter = new OtherEnemyDetectorToSlap.InitParameters();
            oedSlapInitParameter.ForceAmount = forceAmount;
            oedSlapInitParameter.ForceMode = forceMode;

            _slapDetector.Init(oedSlapInitParameter);

            _myBody.AddForce(dir * forceAmount, forceMode);
            _myBody.AddTorque(dir * forceAmount, forceMode);

            StartCoroutine(AddFoceCO(dir, forceAmount, forceMode));

            timer = 0f;
            DOTween.To(() => timer, (x) => timer = x, 1f, _otherDetectorOpenAfterSeconds)
                .OnComplete(() => _slapDetector.gameObject.SetActive(true));

            DOTween.To(() => timer, (x) => timer = x, 10f, _otherDetectorOpenAfterSeconds + 0.5f)
                .OnComplete(() => _slapDetector.gameObject.SetActive(false));
            _myBody.useGravity = true;

            transform.SetParent(_parentSettings.ParentTransform);
            // _dynamicBone.m_Stiffness = 0.15f;
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

        public void AddRagdollBodies(Rigidbody[] ragdollBodies, RagdollMarker marker)
        {
            _ragdollBodies = ragdollBodies;
            _ragdoll = marker.gameObject;
        }

        public class InitParameters
        {
            public ParentSettings ParentSettings { get; set; } 
        }

        [System.Serializable]
        public class ParentSettings
        {
            [SerializeField] string _justTest;
            Transform _parentTransform;

            public Transform ParentTransform
            {
                get
                {
                    if (_parentTransform == null)
                        _parentTransform = new GameObject("RagDoll_Parent").transform;

                    return _parentTransform;
                }
            }
        }
    }
}