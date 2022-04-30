using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RoadSetter : MonoBehaviour
{
    [SerializeField] Settings _settings;

    Transform _roadTransform;

    public void Init(InitParameters initParameters)
    {
        _roadTransform = initParameters.RoadTransform;

        _roadTransform.localScale = _settings.RoadLocalScale;
        _roadTransform.localPosition = _settings.RoadLocalPosition;
        initParameters.EndPlatformTransform.localPosition = _settings.EndPosition;
        var lastIndexOfPath = initParameters.CinemachineSmoothPath.m_Waypoints.Length - 1;
        initParameters.CinemachineSmoothPath.m_Waypoints[lastIndexOfPath].position = _settings.EndPosition;

        var renderer = _roadTransform.GetComponent<MeshRenderer>();
        var materials = renderer.materials;
        materials[1] = _settings.RoadMaterial;
        renderer.materials = materials;
    }

    public class InitParameters
    {
        public Transform RoadTransform { get; set; }
        public Transform EndPlatformTransform { get; set; }
        public CinemachineSmoothPath CinemachineSmoothPath { get; set; }
    }

    [System.Serializable]
    public class Settings
    {
        [SerializeField] Vector3 _roadLocalScale;
        [SerializeField] Vector3 _roadLocalPosition;
        [SerializeField] Vector3 _endLocalPosition;
        [SerializeField] Vector3 _pathEndPosition;
        [SerializeField] Material _roadMaterial;

        public Vector3 RoadLocalScale => _roadLocalScale;
        public Vector3 RoadLocalPosition => _roadLocalPosition;
        public Vector3 EndPosition => _endLocalPosition;
        public Vector3 PathEndPosition => _pathEndPosition;
        public Material RoadMaterial => _roadMaterial;
    }
}
