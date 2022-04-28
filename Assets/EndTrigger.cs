using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class EndTrigger : MonoBehaviour
    {
        [SerializeField] InputManager _inputManager;
        bool _isFinished;
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Other Name: " + other.name);
            other.GetComponentInChildren<Animator>().SetTrigger("idle");

            _inputManager.gameObject.SetActive(false);

            if (_isFinished) return;
            _isFinished = true;

            RoosterHub.Central.Win();
        }
    }
}
