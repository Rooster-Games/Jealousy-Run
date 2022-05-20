using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class BarMovementController : MonoBehaviour
    {
        [SerializeField] Transform _followTransform;
        [SerializeField] Vector3 _placementOffset;

        RectTransform _myRectTransform;

        Vector3 _offset;
        private void Awake()
        {
            _myRectTransform = GetComponent<RectTransform>();

        }

        private void Update()
        {
            _myRectTransform.eulerAngles = _myRectTransform.parent.eulerAngles + _myRectTransform.parent.eulerAngles * -1f;
        }
    }
}
