using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    [CreateAssetMenu(fileName = "_crowdedSpawnData", menuName = "JR/CrowdedCreator/SpawnData")]
    public class CrowdedCreatorSpawnSettingsSO : ScriptableObject
    {
        [SerializeField] GameObject[] _prefabCollection;
        [SerializeField] Vector3 _eulerRotation;
        [SerializeField] float _cellSize = 0.25f;

        public Vector3 EulerRotation => _eulerRotation;
        public float CellSize => _cellSize;
        public GameObject GetRandomPrefab
        {
            get
            {
                int randomIndex = Random.Range(0, _prefabCollection.Length);
                return _prefabCollection[randomIndex];
            }
        }
    }
}