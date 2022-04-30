using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace JR
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField] GameType _gameType;
        [SerializeField] CrowdedCreator _crowdedCreator;
        [SerializeField] BoxCollider _testCol;
        [SerializeField] Settings _settings;
        [SerializeField] float _wayDistance;

        public List<Transform> _createdList = new List<Transform>();

        float _crowdedUnitDistance;

        [Button("Test")]
        public void CreateLevel()
        {
            _crowdedUnitDistance = _crowdedCreator.GetComponent<BoxCollider>().bounds.extents.z * 2f;


            for(int i = 0; i < _settings.MinBothGenderCount * 2; i++)
            {
                CreateCrowded((Gender)(i % 2));
            }

            int levelIndex = RoosterHub.Central.GetLevelNo();
            int levelSelectionIndex = levelIndex >= _settings.LevelCrowdedCount.Length - 1 ? _settings.LevelCrowdedCount.Length - 1 : levelIndex;
            int remainingCreation = _settings.LevelCrowdedCount[levelSelectionIndex] - _settings.MinBothGenderCount * 2;
            int maxCreation = _settings.MinBothGenderCount * 2 + remainingCreation;

            float distance = _crowdedUnitDistance * maxCreation;
            _wayDistance -= Random.Range(_settings.MinMaxEndSpacing.x, _settings.MinMaxEndSpacing.y);
            _wayDistance -= Random.Range(_settings.MinMaxStartSpacing.x, _settings.MinMaxStartSpacing.y);
            _wayDistance -= distance;

            float unitDistance = _wayDistance / maxCreation;

            for (int i = 0; i < remainingCreation; i++)
            {
                Gender randomGender = (Gender)Random.Range(0, 2);
                CreateCrowded(randomGender);
            }
        }

        private void CreateCrowded(Gender gender)
        {
            var ccInitParameters = new CrowdedCreator.InitParameters();
            ccInitParameters.GenderToCreate = gender;
            var crowdedCreator = Instantiate(_crowdedCreator);

            crowdedCreator.Init(ccInitParameters);

            _createdList.Add(crowdedCreator.transform);
        }

        [System.Serializable]
        public class Settings
        {
            [SerializeField] int _minBothGenderCount = 2;
            [SerializeField] int[] _levelCrowdedCount;
            [SerializeField] Vector2 _minMaxStartSpacing;
            [SerializeField] Vector2 _minMaxEndSpacing;
            [SerializeField] Vector2 _minMaxSpacing;

            public int MinBothGenderCount => _minBothGenderCount;
            public int[] LevelCrowdedCount => _levelCrowdedCount;
            public Vector2 MinMaxStartSpacing => _minMaxStartSpacing;
            public Vector2 MinMaxSpacing => _minMaxSpacing;
            public Vector2 MinMaxEndSpacing => _minMaxEndSpacing;
        }
    }
}
