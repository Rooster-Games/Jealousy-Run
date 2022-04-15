using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "JR/PlayerSettings")]
    public class PlayerSettingsSO : ScriptableObject
    {
        [SerializeField] DollyCartController.Settings _dollyCartSettings;

        public DollyCartController.Settings DollyCartSettings => _dollyCartSettings;
    }
}