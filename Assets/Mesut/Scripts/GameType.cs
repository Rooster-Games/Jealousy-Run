
using UnityEngine;
using Cinemachine;
using GameCores;

namespace JR
{
    public class GameType : MonoBehaviour
    {
        Gender _protectorsGender;
        [SerializeField] Transform _roadTransform;
        [SerializeField] Transform _endPlatformTransform;
        [SerializeField] Transform _peopleTransform;
        [SerializeField] CinemachineSmoothPath _smoothpath;
        [SerializeField] CinemachineDollyCart _dollyCart;

        public Gender ProtectorGender => _protectorsGender;

        public CinemachineSmoothPath SmoothPath { get => _smoothpath; set => _smoothpath = value; }

        public void Init(InitParameters initParameters)
        {
            _protectorsGender = initParameters.ProtectorsGender;

            var roadSetterGO = Instantiate(initParameters.LevelPrefab);
            roadSetterGO.transform.position = _peopleTransform.position;
            roadSetterGO.transform.SetParent(_peopleTransform);
            roadSetterGO.SetActive(true);
            var roadSetter = roadSetterGO.GetComponent<RoadSetter>();
            var roadSetterInitParameters = new RoadSetter.InitParameters();
            roadSetterInitParameters.RoadTransform = _roadTransform;
            roadSetterInitParameters.EndPlatformTransform = _endPlatformTransform;
            roadSetterInitParameters.CinemachineSmoothPath = _smoothpath;
            roadSetterInitParameters.DollyCart = _dollyCart;
            roadSetter.Init(roadSetterInitParameters);


            // item creator
            var collectCreatorCollection = roadSetterGO.GetComponentsInChildren<CollectableCreator>();
            foreach (var collectCreator in collectCreatorCollection)
            {
                collectCreator.Init();
                collectCreator.GetComponent<MeshRenderer>().enabled = false;
            }

            // Item collection
            var itemCRCollection = roadSetterGO.GetComponentsInChildren<ItemCompositionRoot>();
            var itemCRInitParameters = new ItemCompositionRoot.InitParameters();
            itemCRInitParameters.WhoIsProtecting = initParameters.ProtectorsGender;

            foreach (var itemCR in itemCRCollection)
            {
                itemCR.Init(itemCRInitParameters);
            }
        }

        public class InitParameters
        {
            public Gender ProtectorsGender { get; set; }
            public GameObject LevelPrefab { get; set; }
            public IEventBus EventBus { get; set; }
        }
    }
}