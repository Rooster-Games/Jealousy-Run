
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
            _animatorController.SetFloat("walkIndex", Random.Range(0, 7));
            _animatorController.SetTrigger("walk");
        }

        public class InitParameters
        {

        }
    }
}