using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class PersonController : MonoBehaviour
    {
        PersonAnimatorController _animatorController;

        private void Awake()
        {
            Init(null);
        }

        public void Init(InitParameters initParameters)
        {
            _animatorController = GetComponentInChildren<PersonAnimatorController>();
        }

        public void Walk()
        {
            _animatorController.SetTrigger("walk");
        }

        public class InitParameters
        {

        }

    }
}