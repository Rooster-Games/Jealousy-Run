using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class GenderInfo : MonoBehaviour
    {
        [SerializeField] Gender _gender;

        public Gender Gender => _gender;
    }
}