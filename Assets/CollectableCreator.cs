using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JR
{
    public class CollectableCreator : MonoBehaviour
    {
        [SerializeField] CrowdedCreatorSpawnSettingsSO _spawnSettings;
        [SerializeField] int _peopleCount = 5;
        [SerializeField] GameObject _collectablePrefab;
        [SerializeField] float _itemRadius = 0.75f;

        BoxCollider _boxCollider;

        //private void Awake()
        //{
        //    Init();
        //}

        List<Vector3> _createdPositions;

        [Button("Init")]
        public void Init()
        {
            _creationCounter = 0;
            _createdPositions = new List<Vector3>();
            _boxCollider = GetComponent<BoxCollider>();

            if (_boxCollider.size != Vector3.one)
            {
                transform.localScale = _boxCollider.size;
                _boxCollider.size = Vector3.one;
            }

            //Vector3 center = _boxCollider.bounds.center;
            Vector3 center = transform.position;
            Vector3 extents = _boxCollider.bounds.extents;

            float width = extents.x - _spawnSettings.CellSize;
            float height = extents.z - _spawnSettings.CellSize;

            int itemCount = (int)((2f * height) / _itemRadius);

            // world pos
            Vector3 leftBottomPos = center + new Vector3(-width, 0f, -height);
            Vector3 rightTopPos = center + new Vector3(width, 0f, height);

            Vector3 bottom = center + new Vector3(0f, 0f, -height);
            Vector3 top = center + new Vector3(0f, 0f, height);

            _boxCollider.size = transform.localScale;
            transform.localScale = Vector3.one;

            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            int checkCounter = 0;
            for (int i = 0; i < itemCount; i++)
            {
                //Vector3 randomPos = GetRandomPosition(leftBottomPos, rightTopPos);
                float percent = (float)i / (itemCount - 1);
                Vector3 randomPos = Vector3.Lerp(bottom, top, percent);

                CreateGo(randomPos);
                //bool canSpawn = CheckIfCanSpawn(randomPos);
                //while (!canSpawn)
                //{
                //    checkCounter++;
                //    if (checkCounter == 10)
                //        break;

                //    randomPos = GetRandomPosition(leftBottomPos, rightTopPos);
                //    canSpawn = CheckIfCanSpawn(randomPos);
                //}

                //if (canSpawn)
                //{
                //    randomPos.x = center.x;
                //    CreateGo(randomPos);
                //}
                //checkCounter = 0;
            }

            // Debug.Log($"Creation Counter: {_creationCounter}");

            _boxCollider.enabled = false;
        }

        private Vector3 GetRandomPosition(Vector3 leftBottomPos, Vector3 rightTopPos)
        {
            float xPos = Random.Range(leftBottomPos.x, rightTopPos.x);
            float yPos = Random.Range(leftBottomPos.z, rightTopPos.z);

            return new Vector3(0f, 0f, yPos);
        }

        private int _creationCounter;
        private void CreateGo(Vector3 pos)
        {
            _creationCounter++;
            var go = Instantiate(_collectablePrefab);
            go.transform.SetParent(transform);
            go.transform.localScale = Vector3.one * Random.Range(_spawnSettings.MinMaxScale.x, _spawnSettings.MinMaxScale.y);
            go.transform.position = pos;
            go.transform.localEulerAngles = _spawnSettings.EulerRotation;
            go.layer = gameObject.layer;
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