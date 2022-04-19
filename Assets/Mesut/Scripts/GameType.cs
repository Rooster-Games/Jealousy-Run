using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class GameType : MonoBehaviour
    {
        [SerializeField] Gender _protectorsGender;

        public Gender ProtectorGender => _protectorsGender;
    }
}