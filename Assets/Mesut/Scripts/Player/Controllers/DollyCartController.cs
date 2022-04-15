using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace JR
{
    public class DollyCartController : MonoBehaviour
    {
        [SerializeField] Settings _settings;
        CinemachineDollyCart _dollyCart;

        public void Init(InitParameters initParameters)
        {
            _settings = initParameters.Settings;
            _dollyCart = initParameters.DollyCart;
        }

        public class InitParameters
        {
            public Settings Settings { get; set; }
            public CinemachineDollyCart DollyCart { get; set; }
        }

        public void StartMoving()
        {
            _dollyCart.m_Speed = _settings.Speed;
        }

        public void StopMoving()
        {
            _dollyCart.m_Speed = 0f;
        }

        public void ChangeCurrentSpeed(float speed)
        {
            _dollyCart.m_Speed = speed;
        }

        [System.Serializable]
        public class Settings
        {
            [SerializeField] float _speed;

            public float Speed => _speed;
        }
    }
}