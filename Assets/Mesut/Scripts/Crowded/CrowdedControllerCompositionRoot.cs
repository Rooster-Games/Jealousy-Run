using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DI;
using GameCores;
using UnityEngine;

namespace JR
{
    public class CrowdedControllerCompositionRoot : MonoBehaviour
    {
        [SerializeField] DollyCartController.Settings _dollycartSettings;
        //public void RegisterToContainer()
        //{
        //    var crowdedController = GetComponent<CrowdedController>();
        //    var dollyCartController = GetComponent<DollyCartController>();
        //    var dollyCart = GetComponent<CinemachineDollyCart>();

        //    PMContainer.Instance.RegisterForIniting(crowdedController);
        //    PMContainer.Instance.RegisterWhenInjecTo(dollyCartController, crowdedController);

        //    PMContainer.Instance.RegisterForIniting(dollyCartController);
        //    PMContainer.Instance.RegisterWhenInjecTo(dollyCart, dollyCartController);
        //    PMContainer.Instance.RegisterWhenInjecTo(_dollycartSettings, dollyCartController);
        //}

        public void Init(InitParameters initParameters)
        {
            // self
            var dollyCart = GetComponent<CinemachineDollyCart>();
            var dollyCartController = GetComponent<DollyCartController>();
            var crowdedController = GetComponent<CrowdedController>();

            //child

            // dollycart controller init
            var dcInitParameters = new DollyCartController.InitParameters();
            dcInitParameters.DollyCart = dollyCart;
            dcInitParameters.Settings = _dollycartSettings;

            dollyCartController.Init(dcInitParameters);

            // crowded controller init
            var ccInitParameters = new CrowdedController.InitParameters();
            ccInitParameters.DollyCartController = dollyCartController;
            ccInitParameters.EventBus = initParameters.EventBus;

            crowdedController.Init(ccInitParameters);

        }

        public class InitParameters
        {
            public EventBus EventBus { get; set; }
        }
    }
}
