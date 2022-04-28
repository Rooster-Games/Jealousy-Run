using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JR
{
    public enum EmojiType { Hard, Medium, Low }
    public class EmojiController : MonoBehaviour
    {
        [SerializeField] GameObject[] _hardCollection;
        [SerializeField] GameObject[] _mediumCollection;
        [SerializeField] GameObject[] _lowCollection;
        [SerializeField] float _destroySeconds = 2f;
        [SerializeField] int _crowdedEmojiCountMax = 3;

        Dictionary<EmojiType, List<GameObject>> _emojiTypeToGOMap = new Dictionary<EmojiType, List<GameObject>>();

        private void Awake()
        {
            AddEmojiToDict(EmojiType.Hard, _hardCollection.ToList());
            AddEmojiToDict(EmojiType.Medium, _mediumCollection.ToList());
            AddEmojiToDict(EmojiType.Low, _lowCollection.ToList());
        }

        public void CreateEmoji(EmojiType type, EmojiRootMarker emojiRootMarker)
        {
            var prefab = GetRandomEmoji(type);
            emojiRootMarker.AddEmoji(prefab, _destroySeconds);
        }

        public void CreateEmojiAtCrowded(EmojiType type, List<Collider> _crowdedCollection)
        {
            int iteration = _crowdedEmojiCountMax < _crowdedCollection.Count ? _crowdedEmojiCountMax : _crowdedCollection.Count;

            for(int i = 0; i < iteration; i++)
            {
                int randomIndex = Random.Range(0, _crowdedCollection.Count);
                var col = _crowdedCollection[randomIndex];
                _crowdedCollection.RemoveAt(randomIndex);

                var emojiRootMarker = col.gameObject.GetComponentInChildren<EmojiRootMarker>();
                Debug.Log("EmojiRootMarker Is Null: " + emojiRootMarker == null);
                CreateEmoji(type, emojiRootMarker);
            }
        }

        private GameObject GetRandomEmoji(EmojiType type)
        {
            var collection = _emojiTypeToGOMap[type];
            var randomIndex = Random.Range(0, collection.Count);
            return collection[randomIndex];
        }

        private void AddEmojiToDict(EmojiType type, List<GameObject> collection)
        {
            _emojiTypeToGOMap.Add(type, collection);
        }
    }
}