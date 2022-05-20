
using Cinemachine;
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

            var personControllerCollection = GetComponentsInChildren<PersonController>();

            var pcInitParameters = new PersonController.InitParameters();
            pcInitParameters.EventBus = initParameters.EventBus;
            foreach (var personController in personControllerCollection)
            {
                personController.Init(pcInitParameters);
            }
        }

        public class InitParameters
        {
            public IEventBus EventBus { get; set; }
        }
    }
}
