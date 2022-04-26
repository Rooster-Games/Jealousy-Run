using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class TestOnCollisionEnter : MonoBehaviour
    {
        TestOnCollisionEnterCompositionRoot _root;

        public void Init(TestOnCollisionEnterCompositionRoot root )
        {
            _root = root;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var direction = collision.GetContact(0).normal;
            Debug.Log("OnCollisionEnter");
            Debug.Log(collision.gameObject.layer);
            if(collision.transform.tag != transform.tag)
                _root.AddForce(direction);
        }
    }
}
