using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameCores;

namespace JR
{
    public enum EmojiType { Hard, Medium, Low }
    public class EmojiController : MonoBehaviour
    {
        [SerializeField] GenderEmojiSettings[] _genderEmojiSettings;

        [SerializeField] GameObject[] _hardCollection;
        [SerializeField] GameObject[] _mediumCollection;
        [SerializeField] GameObject[] _lowCollection;
        [SerializeField] float _destroySeconds = 2f;
        [SerializeField] int _crowdedEmojiCountMax = 3;

        Dictionary<EmojiType, List<GameObject>> _emojiTypeToGOMap = new Dictionary<EmojiType, List<GameObject>>();

        GenderToGenderEmojiController _genderToGenderEmojiController;
        private void Awake()
        {
            AddEmojiToDict(EmojiType.Hard, _hardCollection.ToList());
            AddEmojiToDict(EmojiType.Medium, _mediumCollection.ToList());
            AddEmojiToDict(EmojiType.Low, _lowCollection.ToList());



            Dictionary<Gender, Dictionary<Gender, List<GameObject>>> genderToOtherGenderMap = new Dictionary<Gender, Dictionary<Gender, List<GameObject>>>();
            foreach (var settings in _genderEmojiSettings)
            {
                if (!genderToOtherGenderMap.ContainsKey(settings.Gender))
                {
                    genderToOtherGenderMap.Add(settings.Gender, new Dictionary<Gender, List<GameObject>>());
                }

                genderToOtherGenderMap[settings.Gender].Add(settings.OtherGender, settings.ReationToGenderEmojiCollection.ToList());
            }

            var gtgecInitParameters = new GenderToGenderEmojiController.InitParameters();
            gtgecInitParameters.EmojiDestroySeconds = _destroySeconds;
            gtgecInitParameters.GenderToGenderToGOMap = genderToOtherGenderMap;
            _genderToGenderEmojiController = new GenderToGenderEmojiController();
            _genderToGenderEmojiController.Init(gtgecInitParameters);
        }

        public void CreateEmoji(EmojiType type, EmojiRootMarker emojiRootMarker, Gender myGender, Gender otherGender)
        {
            if (_genderToGenderEmojiController.CreateEmoji(myGender, otherGender, emojiRootMarker)) return;

            CreateEmoji(type, emojiRootMarker);
        }

        private void CreateEmoji(EmojiType type, EmojiRootMarker emojiRootMarker)
        {
             var prefab = GetRandomEmoji(type);
             emojiRootMarker.AddEmoji(prefab, _destroySeconds);
        }    

        public void CreateEmojiAtCrowded(EmojiType type, List<Collider> _crowdedCollection, Gender myGender, Gender otherGender)
        {
            int iteration = _crowdedEmojiCountMax < _crowdedCollection.Count ? _crowdedEmojiCountMax : _crowdedCollection.Count;

            for(int i = 0; i < iteration; i++)
            {
                int randomIndex = Random.Range(0, _crowdedCollection.Count);
                var col = _crowdedCollection[randomIndex];
                _crowdedCollection.RemoveAt(randomIndex);

                var emojiRootMarker = col.gameObject.GetComponentInChildren<EmojiRootMarker>();
                CreateEmoji(type, emojiRootMarker, myGender, otherGender);
            }
        }

        private GameObject GetRandomElementAtList(List<GameObject> collection)
        {
            var randomIndex = Random.Range(0, collection.Count);
            return collection[randomIndex];
        }

        private GameObject GetRandomEmoji(EmojiType type)
        {
            var collection = _emojiTypeToGOMap[type];

            var obj = GetRandomElementAtList(collection);
            return obj;
        }

        private void AddEmojiToDict(EmojiType type, List<GameObject> collection)
        {
            _emojiTypeToGOMap.Add(type, collection);
        }

        [System.Serializable]
        public class GenderEmojiSettings
        {
            [SerializeField] Gender _myGender;
            [SerializeField] Gender _otherGender;
            [SerializeField] GameObject[] _reactionToGenderEmojiCollection;

            public Gender Gender => _myGender;
            public Gender OtherGender => _otherGender;
            public GameObject[] ReationToGenderEmojiCollection => _reactionToGenderEmojiCollection;
        }

        public class GenderToGenderEmojiController
        {
            Dictionary<Gender, Dictionary<Gender, List<GameObject>>> _genderToGenderTOGOMap;
            float _destorySeconds;
            public void Init(InitParameters initParameters)
            {
                _genderToGenderTOGOMap = initParameters.GenderToGenderToGOMap;
                _destorySeconds = initParameters.EmojiDestroySeconds;
            }

            public bool CreateEmoji(Gender myGender, Gender otherGender, EmojiRootMarker emojiRootMarker)
            {
                if (!_genderToGenderTOGOMap.ContainsKey(myGender)) return false;
                if (!_genderToGenderTOGOMap[myGender].ContainsKey(otherGender)) return false;

                var prefab = _genderToGenderTOGOMap[myGender][otherGender].GetRandomItem();
                emojiRootMarker.AddEmoji(prefab, _destorySeconds);

                return true;
            }

            public class InitParameters
            {
                public Dictionary<Gender, Dictionary<Gender, List<GameObject>>> GenderToGenderToGOMap { get; set; }
                public float EmojiDestroySeconds { get; set; }
            }
        }
    }
}