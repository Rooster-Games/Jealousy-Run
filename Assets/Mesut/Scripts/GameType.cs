
using UnityEngine;
using Cinemachine;

namespace JR
{
    public class GameType : MonoBehaviour
    {
        [SerializeField] Gender _protectorsGender;
        [SerializeField] Transform _roadTransform;
        [SerializeField] Transform _endPlatformTransform;
        [SerializeField] Transform _peopleTransform;
        [SerializeField] CinemachineSmoothPath _smoothpath;

        public Gender ProtectorGender => _protectorsGender;

        public void Init(InitParameters initParameters)
        {
            _protectorsGender = initParameters.ProtectorsGender;

            var roadSetterGO = Instantiate(initParameters.LevelPrefab);
            roadSetterGO.transform.position = _peopleTransform.position;
            roadSetterGO.transform.SetParent(_peopleTransform);
            var roadSetter = roadSetterGO.GetComponent<RoadSetter>();
            var roadSetterInitParameters = new RoadSetter.InitParameters();
            roadSetterInitParameters.RoadTransform = _roadTransform;
            roadSetterInitParameters.EndPlatformTransform = _endPlatformTransform;
            roadSetterInitParameters.CinemachineSmoothPath = _smoothpath;
            roadSetter.Init(roadSetterInitParameters);
        }

        public class InitParameters
        {
            public Gender ProtectorsGender { get; set; }
            public GameObject LevelPrefab { get; set; }
        }
    }
}