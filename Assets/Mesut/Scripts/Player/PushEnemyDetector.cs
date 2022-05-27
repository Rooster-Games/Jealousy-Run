using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DG.Tweening;
using GameCores;
using GameCores.CoreEvents;
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
        [SerializeField] float _dotTreshold = 0.75f;

        [SerializeField] EmojiController _emojiController;
        [SerializeField] LayerMask _emojiMask;
        [SerializeField] BarController _barController;
        [SerializeField] Gender _gender;

        float _barChangeValue = 0.033f;
        Dictionary<Collider, int> _colliderMap = new Dictionary<Collider, int>();

        BoxCollider _myCollider;
        public void Init(InitParameters initParameters)
        {
            _barChangeValue = initParameters.BarChangingSettings.DecreaseSettings.OnEncounterWithOppsiteGenderDecreaseValue;
            _myCollider = initParameters.MyBoxCollider;
            _myCollider.enabled = false;

            initParameters.EventBus.Register<OnGameStarted>(EventBus_OnGameStarted);
            initParameters.EventBus.Register<OnBarEmpty>(EventBus_OnBarEmpty);
        }

        private void EventBus_OnGameStarted(OnGameStarted eventData)
        {
            _myCollider.enabled = true;
        }

        bool _isGameEnd;
        private void EventBus_OnBarEmpty(OnBarEmpty eventData)
        {
            _isGameEnd = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_colliderMap.ContainsKey(other)) return;

            _colliderMap.Add(other, 0);

            var pushable = other.GetComponent<Pushable>();
            var otherGenderInfo = other.GetComponent<GenderInfo>();
            if (pushable == null) return;

            if (_gender != otherGenderInfo.Gender)
            {
                _barController.ChangeAmount(-_barChangeValue);
            }

            var otherPos = other.transform.position;
            var myPos = transform.position;
            myPos.y = otherPos.y;

            var otherLookAt = myPos;
            otherLookAt.y = other.transform.position.y;

            other.transform.LookAt(otherLookAt);

            var dir = (otherPos - myPos).normalized;

            var dot = Vector3.Dot(dir, transform.forward);

            if(dot >= _dotTreshold)
            {
                float signX = Mathf.Sign(dir.x);
                dir = new Vector3(dir.z * signX, 0, dir.x * signX);
            }

            List<Collider> colliders = Physics.OverlapSphere(myPos, 2f, _emojiMask).ToList();

            if (colliders.Contains(other))
                colliders.Remove(other);

            var emojiRootMarker = other.gameObject.GetComponentInChildren<EmojiRootMarker>();
            _emojiController.CreateEmoji(EmojiType.Medium, emojiRootMarker, _gender, otherGenderInfo.Gender);
            _emojiController.CreateEmojiAtCrowded(EmojiType.Low, colliders, _gender, otherGenderInfo.Gender);

            pushable.Push(dir, _forceAmount, _forceMode, _gender, otherGenderInfo.Gender);

            //_anim.SetLayerWeight(1, _weight);
            //_anim.ResetTrigger("hit");
            //_anim.SetTrigger("hit");
            //if(!_isReturning)
            //    StartCoroutine(ReturnBackWeight());

            // if (!_isChanging)
            //StartCoroutine(IndexChange());

            if (_isGameEnd)
                other.transform.DOLocalRotate(Vector3.up * 180f, 0.25f).SetDelay(0.75f);
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

        public class InitParameters
        {
            public BarChangingSettings BarChangingSettings { get; set; }
            public IEventBus EventBus { get; set; }
            public BoxCollider MyBoxCollider { get; set; }
        }
    }
}