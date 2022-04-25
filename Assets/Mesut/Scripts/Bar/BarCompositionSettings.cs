using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JR
{
    [System.Serializable]
    public class BarCompositionSettings
    {
        [SerializeField] BarController _barController;
        [SerializeField] BaseBar _bar;
        [SerializeField] LoveData _loveData;
        [SerializeField] float _startingPercent = 0.5f;

        public BarController BarController => _barController;
        public BaseBar Bar => _bar;
        public LoveData LoveData => _loveData;
        public float StartingPercent => _startingPercent;
    }
}