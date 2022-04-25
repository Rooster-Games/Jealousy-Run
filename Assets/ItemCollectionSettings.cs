using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class ItemCollectionSettings : MonoBehaviour
    {
        [SerializeField] float _barIncreaseAmount = 0.1f;

        public float BarIncreaseAmount => _barIncreaseAmount;
    }
}
