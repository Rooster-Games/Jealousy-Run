using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JR
{
    public class CrowdedCreator : MonoBehaviour
    {
        [SerializeField] CrowdedCreatorSpawnSettingsSO _spawnSettings;
        [SerializeField] Vector2Int _minMaxPeopleCount;
        [SerializeField] Gender _creationGender;

        BoxCollider _boxCollider;
        
        //private void Awake()
        //{
        //    Init();
        //}

        List<Vector3> _createdPositions;

        public void Init(InitParameters initParameters)
        {
            if (Mathf.Approximately(transform.localPosition.x, 1.6f))
            {
                _creationGender = (Gender)(((int)initParameters.GenderToCreate + 1) % 2);
                Debug.Log("Creation Gender: " + _creationGender.ToString());
            }
            else if (Mathf.Approximately(transform.localPosition.x, -1.6f))
            {
                _creationGender = (Gender)(((int)_creationGender + 1) % 2);
            }
                    

            Init();
        }

        [Button("Init")]
        private void Init()
        {
            _creationCounter = 0;
            _createdPositions = new List<Vector3>();
            _boxCollider = GetComponent<BoxCollider>();

            if(_boxCollider.size != Vector3.one)
            {
                transform.localScale = _boxCollider.size;
                _boxCollider.size = Vector3.one;
            }

            //Vector3 center = _boxCollider.bounds.center;
            Vector3 center = transform.position;
            Vector3 extents = _boxCollider.bounds.extents;

            float width = extents.x - _spawnSettings.CellSize;
            float height = extents.z - _spawnSettings.CellSize;

            // world pos
            Vector3 leftBottomPos = center + new Vector3(-width, 0f, -height);
            Vector3 rightTopPos = center + new Vector3(width, 0f, height);

            _boxCollider.size = transform.localScale;
            transform.localScale = Vector3.one;

            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            int checkCounter = 0;
            int peopleCount = Random.Range(_minMaxPeopleCount.x, _minMaxPeopleCount.y + 1);
            for(int i = 0; i < peopleCount; i++)
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

            //Debug.Log($"Creation Counter: {_creationCounter}");
        }

        private Vector3 GetRandomPosition(Vector3 leftBottomPos, Vector3 rightTopPos)
        {
            float xPos = Random.Range(leftBottomPos.x, rightTopPos.x);
            float yPos = Random.Range(leftBottomPos.z, rightTopPos.z);

            return new Vector3(xPos, 0f, yPos);
        }

        private int _creationCounter;
        private void CreateGo(Vector3 pos)
        {
            _creationCounter++;
            var go = Instantiate(_spawnSettings.GetRandomPrefab(_creationGender));
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

        public class InitParameters
        {
            public Gender GenderToCreate { get; set; }
        }

        //private void OnTriggerExit(Collider other)
        //{
        //    var animator = other.GetComponentInChildren<Animator>();

        //    if (animator != null)
        //    {
        //        float timer = 1f;
        //        // DOTween.To(() => timer, (x) => { timer = x; animator.SetLayerWeight(1, x); }, 0f, 0.15f);
        //        animator.ResetTrigger("slap");
        //    }
        //}
    }
}