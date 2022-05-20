using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;

namespace JR
{
    public class AnimationTesting : MonoBehaviour
    {
        int _animationCount = 7;
        Animator[] _animators;
        private void Awake()
        {
            _animators = GetComponentsInChildren<Animator>();
        }

        [Button("TestSlapAnimation")]
        public void TestAnimation()
        {
            foreach (var animator in _animators)
            {
                animator.SetLayerWeight(1, 1f);
                animator.SetFloat("tokatIndex", Random.Range(0, _animationCount));
                animator.SetTrigger("slap");
            }

            StartCoroutine(RestartAnimatorWeight());
        }

        IEnumerator RestartAnimatorWeight()
        {
            yield return new WaitForSeconds(1f);
            foreach (var animator in _animators)
            {
                float timer = 1f;
                // DOTween.To(() => timer, (x) => { timer = x; animator.SetLayerWeight(1, x); }, 0f, 0.25f);
            }
        }
    }
}

