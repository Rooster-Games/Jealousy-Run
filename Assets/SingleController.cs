using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace JR
{
    public enum Gender { Male, Female }
    public class SingleController : MonoBehaviour
    {
        [SerializeField] GenderInfo _genderInfo;
        IAnimatorController _animatorController;

        public GenderInfo GenderInfo => _genderInfo;

        bool _isSlapping;
        public bool IsSlapping => _isSlapping;

        public void Init(InitParameters initParameters)
        {
            _animatorController = initParameters.AnimatorController;
        }

        public void Slap()
        {
            _isSlapping = true;
            _animatorController.SetLayerWeight(1, 1f);
            _animatorController.SetTrigger("slap");
            StartCoroutine(RestartAnimatorWeight());
        }

        IEnumerator RestartAnimatorWeight()
        {
            yield return new WaitForSeconds(1f);
            float timer = 1f;
            DOTween.To(() => timer, (x) => { timer = x; _animatorController.SetLayerWeight(1, x); }, 0f, 0.25f)
                .OnComplete(() => _isSlapping = false);
        }

        public class InitParameters
        {
            public IAnimatorController AnimatorController { get; set; }
        }
    }
}