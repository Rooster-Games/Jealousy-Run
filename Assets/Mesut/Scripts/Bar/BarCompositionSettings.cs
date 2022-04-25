using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JR
{
    [System.Serializable]
    public class BarCompositionSettings
    {
        [SerializeField] BarController _barController;
        [SerializeField] BaseBar[] _bars;
        [SerializeField] LoveData _loveData;
        [SerializeField] float _startingPercent = 0.5f;

        public BarController BarController => _barController;
        public BaseBar[] Bars => _bars;
        public LoveData LoveData => _loveData;
        public float StartingPercent => _startingPercent;
    }
}