
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

            var crowdedCreatorInitParameters = new CrowdedCreator.InitParameters();
            crowdedCreatorInitParameters.ProtectorGender = initParameters.ProtectorGender;

            foreach (var crowdedCreator in crowdedCreatorCollection)
            {
                crowdedCreator.Init(crowdedCreatorInitParameters);
                crowdedCreator.GetComponent<MeshRenderer>().enabled = false;
            }

            var personControllerCollection = GetComponentsInChildren<PersonController>();

            var pcInitParameters = new PersonController.InitParameters();
            pcInitParameters.EventBus = initParameters.EventBus;

            var emojiMarkerInitParameters = new EmojiRootMarker.InitParameters();
            emojiMarkerInitParameters.ParticlePool = initParameters.ParticlePool;

            var slapableInitParameters = new Slapable.InitParameters();
            slapableInitParameters.ParentSettings = initParameters.SlapableParentSettings;

            foreach (var personController in personControllerCollection)
            {
                var emojiMarker = personController.GetComponentInChildren<EmojiRootMarker>();
                emojiMarker.Init(emojiMarkerInitParameters);

                personController.Init(pcInitParameters);
                personController.GetComponent<Slapable>().Init(slapableInitParameters);
            }
        }

        public class InitParameters
        {
            public IEventBus EventBus { get; set; }
            public Gender ProtectorGender { get; set; }
            public ParticlePool ParticlePool { get; set; }
            public Slapable.ParentSettings SlapableParentSettings { get; set; }
        }
    }
}