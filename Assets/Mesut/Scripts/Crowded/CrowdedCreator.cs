using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace JR
{
    public class CrowdedCreator : MonoBehaviour
    {
        [SerializeField] CrowdedCreatorSpawnSettingsSO _spawnSettings;
        [SerializeField] int _peopleCount = 5;

        BoxCollider _boxCollider;

        Vector3 _leftTopStarting;
        
        private void Awake()
        {
            Init();
        }

        List<Vector3> _createdPositions;

        [Button("Init")]
        public void Init()
        {
            _createdPositions = new List<Vector3>();
            _boxCollider = GetComponent<BoxCollider>();

            Vector3 center = _boxCollider.bounds.center;
            Vector3 extents = _boxCollider.bounds.extents;

            float width = extents.x - _spawnSettings.CellSize;
            float height = extents.z - _spawnSettings.CellSize;

            // world pos
            Vector3 leftBottomPos = center + new Vector3(-width, 0f, -height);
            Vector3 rightTopPos = center + new Vector3(width, 0f, height);

            while(transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            int checkCounter = 0;
            for(int i = 0; i < _peopleCount; i++)
            {
                Vector3 randomPos = GetRandomPosition(leftBottomPos, rightTopPos);
                bool canSpawn = CheckIfCanSpawn(randomPos);
                while (!canSpawn)
                {
                    checkCounter++;
                    if (checkCounter == 10)
                        break;

                    randomPos = GetRandomPosition(leftBottomPos, rightTopPos);
                    canSpawn = CheckIfCanSpawn(randomPos);
                }

                if(canSpawn)
                {
                    CreateGo(randomPos);
                }
                checkCounter = 0;
            }

        }

        private Vector3 GetRandomPosition(Vector3 leftBottomPos, Vector3 rightTopPos)
        {
            float xPos = Random.Range(leftBottomPos.x, rightTopPos.x);
            float yPos = Random.Range(leftBottomPos.z, rightTopPos.z);

            return new Vector3(xPos, 0f, yPos);
        }

        private void CreateGo(Vector3 pos)
        {
            var go = Instantiate(_spawnSettings.GetRandomPrefab);
            go.transform.SetParent(transform);
            go.transform.position = pos;
            go.transform.localEulerAngles = _spawnSettings.EulerRotation;
        }

        private bool CheckIfCanSpawn(Vector3 spawnPos)
        {
            foreach (var pos in _createdPositions)
            {
                float left = pos.x - _spawnSettings.CellSize;
                float right = pos.x + _spawnSettings.CellSize;
                float bottom = pos.z - _spawnSettings.CellSize;
                float top = pos.z + _spawnSettings.CellSize;

                if (spawnPos.x >= left && spawnPos.x <= right && spawnPos.z >= bottom && spawnPos.z <= top)
                    return false;
            }

            _createdPositions.Add(spawnPos);
            return true;
        }   
    }
}