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
            var dollyCart = GetComponent<CinemachineDollyCart>();

            // Child
            var dollyCartController = GetComponentInChildren<DollyCartController>();
            var playerController = GetComponentInChildren<PlayerController>();

            PMContainer.Instance.RegisterForIniting(dollyCartController);
            PMContainer.Instance.RegisterWhenInjecTo(_playerSettings.DollyCartSettings, dollyCartController);
            PMContainer.Instance.RegisterWhenInjecTo(dollyCart, dollyCartController);


            PMContainer.Instance.RegisterForIniting(playerController);
            PMContainer.Instance.RegisterWhenInjecTo(dollyCartController, playerController);
            PMContainer.Instance.RegisterWhenInjecTo(_playerSettings.PlayerControllerSettings, playerController);



            // TODO:
            // ortak seyler icin ayni objeyi iki kere register etmem gerekiyor
            // bunun icin bir sey bulmam gerekiyor
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