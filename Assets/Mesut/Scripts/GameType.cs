using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class GameType : MonoBehaviour
    {
        [SerializeField] Gender _protectorsGender;

        public Gender ProtectorGender => _protectorsGender;

        public void Init(InitParameters initParameters)
        {
            _protectorsGender = initParameters.ProtectorsGender;
        }

        public class InitParameters
        {
            public Gender ProtectorsGender { get; set; }
        }
    }
}