using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JR
{
    public class PersonAnimatorController : MonoBehaviour
    {
        Animator _animator;
        private void Awake()
        {
            Init(null);
        }

        public void Init(InitParameters initParameters)
        {
            _animator = GetComponent<Animator>();
        }

        public void SetTrigger(string triggerName)
        {
            _animator.SetTrigger(triggerName);
        }

        public class InitParameters
        {

        }
    }
}
