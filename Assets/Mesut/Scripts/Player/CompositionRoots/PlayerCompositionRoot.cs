using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cinemachine;
using DI;
using GameCores;
using UnityEngine;

namespace JR
{
    public class PlayerCompositionRoot : MonoBehaviour
    {
        [SerializeField] PlayerSettingsSO _playerSettings;
        public void RegisterToContainer()
        {
            // CompositionRoot
            //DollyCartCompositionRoot dollyCartCompositionRoot = GetComponent<DollyCartCompositionRoot>();

            // Self
            //var dollyCart = GetComponent<CinemachineDollyCart>();

            //// Child
            var singleControllerCollection = GetComponentsInChildren<SingleController>();
            //var dollyCartController = GetComponentInChildren<DollyCartController>();
            //var playerController = GetComponentInChildren<PlayerController>();
            var animators = GetComponentsInChildren<Animator>();

            var animatorController = new AnimatorControllerFactory().Create(animators);

            //PMContainer.Instance.RegisterForIniting(dollyCartController);
            //PMContainer.Instance.RegisterWhenInjecTo(_playerSettings.DollyCartSettings, dollyCartController);
            //PMContainer.Instance.RegisterWhenInjecTo(dollyCart, dollyCartController);

            //PMContainer.Instance.RegisterForIniting(playerController);
            //PMContainer.Instance.RegisterWhenInjecTo(dollyCartController, playerController);
            //PMContainer.Instance.RegisterWhenInjecTo(_playerSettings.PlayerControllerSettings, playerController);
            //PMContainer.Instance.RegisterWhenInjecTo(animatorController, playerController);

           

            PMContainer.Instance.RegisterSingle(_playerSettings.DollyCartSettings);
            PMContainer.Instance.RegisterSingle(_playerSettings.PlayerControllerSettings);

            PMContainer.Instance.RegisterGameobject(gameObject)
                .RegisterComponent<CinemachineDollyCart>()
                .RegisterComponent<PlayerController>()
                .RegisterComponent<DollyCartController>()
                .RegisterComponentCollection<Animator>()
                .RegisterComponentCollection<TriggerDetector>()
                .RegisterComponentCollection<SingleController>()
                .RegisterWhenInjectoTo<PlayerController, IAnimatorController>(animatorController);

            foreach (var singleController in singleControllerCollection)
            {
                var triggerDetector = singleController.gameObject.GetComponentInChildren<TriggerDetector>();
                PMContainer.Instance.RegisterWhenInjecTo(singleController, triggerDetector);
                PMContainer.Instance.RegisterForIniting(triggerDetector);

                var animator = singleController.GetComponentInChildren<Animator>();
                var singleAnimatorController = new AnimatorControllerFactory().Create(animator);
                PMContainer.Instance.RegisterWhenInjecTo(singleAnimatorController, singleController);

            }

        }
        public void Init(InitParameters initParameters)
        {
            //// CompositionRoot
            //DollyCartCompositionRoot dollyCartCompositionRoot = GetComponent<DollyCartCompositionRoot>();

            //// Self
            //var dollyCart = GetComponent<CinemachineDollyCart>();

            //// Child
            //var dollyCartController = GetComponentInChildren<DollyCartController>();

            //// Creation of InitParameters
            //var dollyCartCompositionRootInitParameters = new DollyCartCompositionRoot.InitParameters();
            //dollyCartCompositionRootInitParameters.DollyCart = dollyCart;
            //dollyCartCompositionRootInitParameters.DollyCartController = dollyCartController;
            //dollyCartCompositionRootInitParameters.DollyCartSettings = _playerSettings.DollyCartSettings;

            //dollyCartCompositionRoot.Init(dollyCartCompositionRootInitParameters);
        }

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
        }
    }
}