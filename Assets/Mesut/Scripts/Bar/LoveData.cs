using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class LoveData : MonoBehaviour
    {
        [SerializeField] float _maxValue = 1f;
        [SerializeField] float _currentValue;

        public float CurrentPercent => (_currentValue / _maxValue);
        public float MaxValue => _maxValue;
        public float CurrentValue { get => _currentValue; set => _currentValue = value; }

        public void Init(InitParameters initParameters)
        {
            _currentValue = initParameters.StartingValue;
        }

        public class InitParameters
        {
            public float StartingValue { get; set; }
        }
    }
}