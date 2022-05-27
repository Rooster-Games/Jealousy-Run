using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    [System.Serializable]
    public class BarChangingSettings
    {
        [field: SerializeField] public IncreaseSettingsC IncreaseSettings { get; private set; }
        [field: SerializeField] public DecreaseSettingsC DecreaseSettings { get; private set; }

        [System.Serializable]
        public class IncreaseSettingsC
        {
            [field: SerializeField] public float WhileProtectingGainPerSeconds { get; private set; }
            [field: SerializeField] public float WhileCollectingAnItemValue { get; private set; }
        }

        [System.Serializable]
        public class DecreaseSettingsC
        {
            [field: SerializeField] public float OnEncounterWithOppsiteGenderDecreaseValue { get; private set; }
        }

    }
}