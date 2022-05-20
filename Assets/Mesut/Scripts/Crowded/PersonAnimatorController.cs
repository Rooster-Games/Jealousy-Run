
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

        public void SetFloat(string name, float value)
        {
            _animator.SetFloat(name, value);
        }

        public class InitParameters
        {

        }
    }
}
