using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "JR/PlayerSettings")]
    public class PlayerSettingsSO : ScriptableObject
    {
        [SerializeField] PlayerController.Settings _playerControllerSettings;
        [SerializeField] DollyCartController.Settings _dollyCartSettings;
        [SerializeField] CoupleTransformSettings _coupleTransformSettings;

        public DollyCartController.Settings DollyCartSettings => _dollyCartSettings;
        public PlayerController.Settings PlayerControllerSettings => _playerControllerSettings;
        public CoupleTransformSettings CoupleTransformSettings => _coupleTransformSettings;

    }

    [System.Serializable]
    public class CoupleTransformSettings
    {
        [SerializeField] Vector3 _protectorStartingPosition = new Vector3(1f, 0f, 0f);
        [SerializeField] Vector3 _defendedStartingPosition = new Vector3(-1f, 0f, 0f);

        public Vector3 ProtectorStartingPosition => _protectorStartingPosition;
        public Vector3 DefendedStartingPosition => _defendedStartingPosition;
    }
}