using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class LevelTestingSettings : MonoBehaviour
    {
        [field: SerializeField] public bool TestThisLevel { get; private set; }
    }
}