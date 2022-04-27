using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using DG.Tweening;
using UnityEngine;

namespace JR
{
    public class PushEnemyDetector : MonoBehaviour
    {
        [SerializeField] ForceMode _forceMode;
        [SerializeField] float _forceAmount = 2500f;
        [SerializeField] Animator _anim;
        [SerializeField] float _weight = 0.65f;
        [SerializeField] float _waitSeconds = 0.45f;
        [SerializeField] float _returnBackDuration = 0.25f;
        [SerializeField] float _indexChangeSeconds = 0.8f;

        private void OnTriggerEnter(Collider other)
        {
            var pushable = other.GetComponent<Pushable>();
            if (pushable == null) return;

            var otherPos = other.transform.position;
            var myPos = transform.position;
            myPos.y = otherPos.y;

            var otherLookAt = myPos;
            otherLookAt.y = other.transform.position.y;

            other.transform.LookAt(otherLookAt);

            var dir = (otherPos - myPos).normalized;
            pushable.Push(dir, _forceAmount, _forceMode);

            _anim.SetLayerWeight(1, _weight);
            _anim.ResetTrigger("hit");
            _anim.SetTrigger("hit");
            if(!_isReturning)
                StartCoroutine(ReturnBackWeight());

            if (!_isChanging)
                StartCoroutine(IndexChange());
        }

        Tween _backTween;

        bool _isReturning;
        bool _isChanging;

        IEnumerator IndexChange()
        {
            _isChanging = true;
            yield return new WaitForSeconds(_indexChangeSeconds);
            _anim.SetFloat("hitIndex", Random.Range(0, 8));
            _isChanging = false;
        }

        IEnumerator ReturnBackWeight()
        {
            _isReturning = true;
            if (_backTween != null)
                _backTween.Kill();

            yield return new WaitForSeconds(_waitSeconds);

            float timer = _weight;
            _backTween = DOTween.To(() => timer, (x) => { timer = x; _anim.SetLayerWeight(1, x); }, 0f, _returnBackDuration);
            _isReturning = false;
        }
    }
}