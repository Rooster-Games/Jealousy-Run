using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DI;
using DIC;
using GameCores;
using UnityEngine;

namespace JR
{
    public class CrowdedControllerCompositionRoot : BaseCompRootGO
    {
        [SerializeField] DollyCartController.Settings _dollycartSettings;

        public override void RegisterToContainer()
        {
            DIContainer.Instance.RegisterGameObject(gameObject)
                .RegisterComponent<CinemachineDollyCart>()
                .RegisterComponent<DollyCartController>()
                .RegisterComponent<CrowdedController>()
                .RegisterWhenInjectTo<DollyCartController>(_dollycartSettings)
                .RegisterComponent<CrowdedControllerCompositionRoot>();
        }

        public void Init(InitParameters initParameters)
        {
            var crowdedCreatorCollection = GetComponentsInChildren<CrowdedCreator>();

            foreach (var crowdedCreator in crowdedCreatorCollection)
            {
                crowdedCreator.Init();
            }
        }

        public class InitParameters
        {

        }
    }
}
