using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "JR/PlayerSettings")]
    public class PlayerSettingsSO : ScriptableObject
    {
        //[SerializeField] PlayerController.Settings _playerControllerSettings;
        [SerializeField] DollyCartController.Settings _dollyCartSettings;
        [SerializeField] CoupleTransformSettings _coupleTransformSettings;
        [SerializeField] DoTweenSwapper.MoveSettings _swapMoveSettings;
        [SerializeField] SpeedChanger.Settings _speedChangerSettings;
        [SerializeField] CameraFovChanger.Settings _cameraFovChangerSettings;
        [SerializeField] ExhaustChecker.Settings _exhaustCheckerSettings;
        [SerializeField] SingleController.AnimatorGenderSettings[] _animatorGenderSettingsCollection;
        [SerializeField] PlayerAnimationEvents.Settings _animationSettings;
        [SerializeField] SingleController.SlapAnimationSettings _slapAnimationSettings;

        public DollyCartController.Settings DollyCartSettings => _dollyCartSettings;
        public DoTweenSwapper.MoveSettings SwapMoveSettings => _swapMoveSettings;
        public SpeedChanger.Settings SpeedChangerSettings => _speedChangerSettings;
        //public PlayerController.Settings PlayerControllerSettings => _playerControllerSettings;
        public CoupleTransformSettings CoupleTransformSettings => _coupleTransformSettings;
        public CameraFovChanger.Settings CameraFovChangerSettings => _cameraFovChangerSettings;
        public ExhaustChecker.Settings ExhaustChecketSettings => _exhaustCheckerSettings;
        public SingleController.AnimatorGenderSettings[] AnimatorGenderSettingsCollection => _animatorGenderSettingsCollection;
        public PlayerAnimationEvents.Settings AnimationSettings => _animationSettings;
        public SingleController.SlapAnimationSettings SlapAnimationSettings => _slapAnimationSettings;

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