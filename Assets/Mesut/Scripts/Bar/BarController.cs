using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace JR
{
    public class BarController : MonoBehaviour
    {
        [SerializeField] BaseBar _bar;
        [SerializeField] LoveData _loveData;

        public void Init(InitParameters initParameters)
        {
            _bar = initParameters.Bar;
            _loveData = initParameters.LoveData;
        }

        [Button("Change")]
        public void ChangeAmount()
        {
            _loveData.CurrentValue += 0.1f;
            _bar.ChangeAmount(_loveData.CurrentPercent);
        }

        public class InitParameters
        {
            public BaseBar Bar { get; set; }
            public LoveData LoveData { get; set; }
        }
    }
}