

using System.Collections.Generic;
using Cinemachine;
using GameCores;
using UnityEngine;
using System.Linq;

namespace JR
{
    public class PlayerCompositionRoot : MonoBehaviour
    {
        [SerializeField] PlayerSettingsSO _playerSettings;
        [SerializeField] CoroutineSwapper _coroutineSwapper;
        //public void RegisterToContainer()
        //{
            //    // CompositionRoot
            //    //DollyCartCompositionRoot dollyCartCompositionRoot = GetComponent<DollyCartCompositionRoot>();

            //    // Self
            //    //var dollyCart = GetComponent<CinemachineDollyCart>();

            //    //// Child
            //    var singleControllerCollection = GetComponentsInChildren<SingleController>();
            //    //var dollyCartController = GetComponentInChildren<DollyCartController>();
            //    //var playerController = GetComponentInChildren<PlayerController>();
            //    var animators = GetComponentsInChildren<Animator>();

            //    var animatorController = new AnimatorControllerFactory().Create(animators);

            //    //PMContainer.Instance.RegisterForIniting(dollyCartController);
            //    //PMContainer.Instance.RegisterWhenInjecTo(_playerSettings.DollyCartSettings, dollyCartController);
            //    //PMContainer.Instance.RegisterWhenInjecTo(dollyCart, dollyCartController);

            //    //PMContainer.Instance.RegisterForIniting(playerController);
            //    //PMContainer.Instance.RegisterWhenInjecTo(dollyCartController, playerController);
            //    //PMContainer.Instance.RegisterWhenInjecTo(_playerSettings.PlayerControllerSettings, playerController);
            //    //PMContainer.Instance.RegisterWhenInjecTo(animatorController, playerController);



            //    PMContainer.Instance.RegisterSingle(_playerSettings.DollyCartSettings);
            //    PMContainer.Instance.RegisterSingle(_playerSettings.PlayerControllerSettings);


            // PMContainer.Instance.RegisterRoot(gameObject);

        //PMContainer.Instance.RegisterGameobject(gameObject)
        //        .RegisterComponent<CinemachineDollyCart>()
        //        .RegisterComponent<PlayerController>()
        //        .RegisterComponent<DollyCartController>()
        //        .RegisterComponentCollection<Animator>()
        //        .RegisterComponentCollection<TriggerDetector>()
        //        .RegisterComponentCollection<SingleController>()
        //        .RegisterWhenInjectoTo<PlayerController, IAnimatorController>(animatorController);

        //    foreach (var singleController in singleControllerCollection)
        //    {
        //        var triggerDetector = singleController.gameObject.GetComponentInChildren<TriggerDetector>();
        //        PMContainer.Instance.RegisterWhenInjecTo(singleController, triggerDetector);
        //        PMContainer.Instance.RegisterForIniting(triggerDetector);

        //        var animator = singleController.GetComponentInChildren<Animator>();
        //        var singleAnimatorController = new AnimatorControllerFactory().Create(animator);
        //        PMContainer.Instance.RegisterWhenInjecTo(singleAnimatorController, singleController);

        //    }

        // }

        public void Init(InitParameters initParameters)
        {
            // CompositionRoot
            DollyCartCompositionRoot dollyCartCompositionRoot = GetComponent<DollyCartCompositionRoot>();
            var singleControllerCompositionRootCollection = GetComponentsInChildren<SingleControllerCompositionRoot>();

            // Self
            var dollyCart = GetComponent<CinemachineDollyCart>();

            // Child
            var dollyCartController = GetComponentInChildren<DollyCartController>();
            var playerController = GetComponentInChildren<PlayerController>();
            var animatorCollection = GetComponentsInChildren<Animator>();
            var singleControllerCollection = GetComponentsInChildren<SingleController>();

            // DollyCartCompositinRoot Init
            var dollyCartCompositionRootInitParameters = new DollyCartCompositionRoot.InitParameters();
            dollyCartCompositionRootInitParameters.DollyCart = dollyCart;
            dollyCartCompositionRootInitParameters.DollyCartController = dollyCartController;
            dollyCartCompositionRootInitParameters.DollyCartSettings = _playerSettings.DollyCartSettings;

            dollyCartCompositionRoot.Init(dollyCartCompositionRootInitParameters);

            IProtector protector = null;
            IGuarded guarded = null;
            Dictionary<GameObject, RuntimeAnimatorController> _singleToRuntimeAnimatorMap = new Dictionary<GameObject, RuntimeAnimatorController>();
            // GameType settings
            foreach (var singleController in singleControllerCollection)
            {
                Vector3 startingLocalPosition = _playerSettings.CoupleTransformSettings.DefendedStartingPosition;
                //var genderInfo = singleController.GetComponent<GenderInfo>();

                if (initParameters.GameType.ProtectorGender == singleController.GenderInfo.Gender)
                {
                    var runTimeAnimatorController = SelectRuntimeAnimatorController(singleController.GenderInfo, true);
                    _singleToRuntimeAnimatorMap.Add(singleController.gameObject, runTimeAnimatorController);

                    startingLocalPosition = _playerSettings.CoupleTransformSettings.ProtectorStartingPosition;
                    Debug.Log(singleController.gameObject.name);

                    var swapperInitParameters = new ISwapper.InitParameters();
                    swapperInitParameters.TransformToSwap = singleController.transform;
                    swapperInitParameters.MoveSettings = _playerSettings.SwapMoveSettings;
                    _coroutineSwapper.Init(swapperInitParameters);

                    protector = singleController;

                    singleController.GetComponentsInChildren<SlapEnemyDetector>(true).ToList().ForEach((x) => x.gameObject.SetActive(true));
                    singleController.GetComponentInChildren<ItemTriggerDetector>(true).gameObject.SetActive(true);
                }
                else
                {
                    var runTimeAnimatorController = SelectRuntimeAnimatorController(singleController.GenderInfo, false);
                    _singleToRuntimeAnimatorMap.Add(singleController.gameObject, runTimeAnimatorController);
                    guarded = singleController;
                }

                singleController.transform.localPosition = startingLocalPosition;
            }

            // SingleControllers composition root init
            var scCRInitparameters = new SingleControllerCompositionRoot.InitParameters();
            scCRInitparameters.MoveSettings = _playerSettings.SwapMoveSettings;
            scCRInitparameters.ExhaustCheckerSettings = _playerSettings.ExhaustChecketSettings;
            scCRInitparameters.Swapper = _coroutineSwapper;

            foreach (var sccr in singleControllerCompositionRootCollection)
            {

                scCRInitparameters.RuntimeAnimatorController = _singleToRuntimeAnimatorMap[sccr.gameObject];

                sccr.Init(scCRInitparameters);
            }


            // camera Fov Changer
            var fovChangerInitParameters = new CameraFovChanger.InitParameters();
            fovChangerInitParameters.Camera = initParameters.CameraToFovChange;
            fovChangerInitParameters.Settings = _playerSettings.CameraFovChangerSettings;

            var cameraFovChanger = new CameraFovChanger();
            cameraFovChanger.Init(fovChangerInitParameters);

            // Player Init
            var animatorController = new AnimatorControllerFactory().Create(animatorCollection);

            var playerControllerInitParameters = new PlayerController.InitParameters();
            playerControllerInitParameters.DollyCartController = dollyCartController;
            playerControllerInitParameters.EventBus = initParameters.EventBus;
            playerControllerInitParameters.InputManager = initParameters.InputManager;
            playerControllerInitParameters.AnimatorController = animatorController;
            playerControllerInitParameters.Protector = protector;
            playerControllerInitParameters.Guarded = guarded;
            playerControllerInitParameters.SpeedChangerSettings = _playerSettings.SpeedChangerSettings;
            playerControllerInitParameters.CameraFovChanger = cameraFovChanger;

            playerController.Init(playerControllerInitParameters);

            // item trigger detectorInit
            var itemTDInitParameters = new ItemTriggerDetector.InitParameters();
            itemTDInitParameters.BarController = initParameters.BarController;

            var itemTriggerDetectorCollection = GetComponentsInChildren<ItemTriggerDetector>();
            foreach (var itemTD in itemTriggerDetectorCollection)
            {
                itemTD.Init(itemTDInitParameters);
            }
        }

        private RuntimeAnimatorController SelectRuntimeAnimatorController(GenderInfo genderInfo, bool isProtector)
        {
            foreach (var animatorSettings in _playerSettings.AnimatorGenderSettingsCollection)
            {
                if (animatorSettings.Gender == genderInfo.Gender)
                {
                    if (isProtector)
                    {
                        //Debug.Log(animatorSettings.ProtectorRunTimeAnimatorController.name);
                        return animatorSettings.ProtectorRunTimeAnimatorController;
                    }

                    return animatorSettings.GuardedAnimatorController;
                }
            }

            throw new System.Exception("Burada sorun var");
        }

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
            public InputManager InputManager { get; set; }
            public GameType GameType { get; set; }
            public BarController BarController { get; set; }
            public CinemachineVirtualCamera CameraToFovChange { get; set; }
        }
    }
}