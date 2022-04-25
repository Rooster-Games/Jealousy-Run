

using Cinemachine;
using GameCores;
using UnityEngine;

namespace JR
{
    public class PlayerCompositionRoot : MonoBehaviour
    {
        [SerializeField] PlayerSettingsSO _playerSettings;
        [SerializeField] DoTweenSwapper.MoveSettings _moveSettings;
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


            // SingleControllers composition root init
            var scCRInitparameters = new SingleControllerCompositionRoot.InitParameters();
            scCRInitparameters.MoveSettings = _moveSettings;

            foreach (var sccr in singleControllerCompositionRootCollection)
            {
                sccr.Init(scCRInitparameters);
            }

            IProtector protector = null;
            // GameType settings
            foreach (var singleController in singleControllerCollection)
            {
                Vector3 startingLocalPosition = _playerSettings.CoupleTransformSettings.DefendedStartingPosition;
                if (initParameters.GameType.ProtectorGender == singleController.GenderInfo.Gender)
                {
                    startingLocalPosition = _playerSettings.CoupleTransformSettings.ProtectorStartingPosition;
                    protector = singleController;
                }

                singleController.transform.localPosition = startingLocalPosition;
            }

            // Player Init
            var animatorController = new AnimatorControllerFactory().Create(animatorCollection);

            var playerControllerInitParameters = new PlayerController.InitParameters();
            playerControllerInitParameters.DollyCartController = dollyCartController;
            playerControllerInitParameters.EventBus = initParameters.EventBus;
            playerControllerInitParameters.InputManager = initParameters.InputManager;
            playerControllerInitParameters.Settings = _playerSettings.PlayerControllerSettings;
            playerControllerInitParameters.AnimatorController = animatorController;
            playerControllerInitParameters.Protector = protector;

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

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
            public InputManager InputManager { get; set; }
            public GameType GameType { get; set; }
            public BarController BarController { get; set; }
        }
    }
}