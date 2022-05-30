using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public class EmojiRootMarker : MonoBehaviour
    {
        bool _hasEmoji;

        ParticlePool _particlePool;

        public void Init(InitParameters initParameters)
        {
            _particlePool = initParameters.ParticlePool;
        }

        public void AddEmoji(GameObject prefab, float destorySeconds)
        {
            if (_hasEmoji) return;

            _hasEmoji = true;
            var emoji = _particlePool.PlayParticle(prefab, transform);
            StartCoroutine(ResetMe(prefab, emoji, destorySeconds));
        }

        IEnumerator ResetMe(GameObject prefab, GameObject emojiObj, float destroySeconds)
        {
            yield return new WaitForSeconds(destroySeconds);

            _particlePool.ReturnBackToPool(prefab, emojiObj);
            //Destroy(emojiObj);
            _hasEmoji = false;

            // 6.06, -6.02
        }

        public class InitParameters
        {
            public ParticlePool ParticlePool { get; set; }
        }
    }

    public class ParticlePool
    {
        Dictionary<GameObject, List<GameObject>> _prefabToPoolledObjMap = new Dictionary<GameObject, List<GameObject>>();

        int _startingPool = 5;

        Transform poolRoot;
        Dictionary<GameObject, Transform> _prefabToPoolParent = new Dictionary<GameObject, Transform>();

        public ParticlePool()
        {
            poolRoot = new GameObject("ParticlePool").transform;
        }

        public void Init(InitParameters initParameters)
        {
            AddPrefabCollection(initParameters.ParticlePoolSettings.ParticlePrefabCollection);
        }

        public void AddPrefab(GameObject prefab)
        {
            if(!_prefabToPoolledObjMap.ContainsKey(prefab))
            {
                _prefabToPoolledObjMap.Add(prefab, new List<GameObject>());

                var prefabPoolParent = new GameObject($"{prefab.name}_Pool").transform;
                prefabPoolParent.SetParent(poolRoot);
                _prefabToPoolParent.Add(prefab, prefabPoolParent);

                for(int i = 0; i < _startingPool; i++)
                {
                    AddPrefabToPool(prefab);
                }
            }
        }

        public void AddPrefabToPool(GameObject prefab)
        {
            var go = GameObject.Instantiate(prefab);
            go.SetActive(false);
            var parent = _prefabToPoolParent[prefab];
            go.transform.SetParent(parent);
            _prefabToPoolledObjMap[prefab].Add(go);
        }

        public void AddPrefabCollection(IEnumerable<GameObject> prefabCollection)
        {
            foreach (var prefab in prefabCollection)
            {
                AddPrefab(prefab);
            }
        }

        private GameObject GetObject(GameObject prefab)
        {
            if(_prefabToPoolledObjMap.TryGetValue(prefab, out var pooledObjList))
            {
                foreach (var poolledObj in pooledObjList)
                {
                    if (!poolledObj.activeInHierarchy)
                    {
                        poolledObj.SetActive(true);
                        return poolledObj;
                    }
                }

                for (int i = 0; i < _startingPool; i++)
                    AddPrefabToPool(prefab);

                return GetObject(prefab);
            }

            Debug.LogError($"Returning null: {prefab.name}");
            return null;
        }

        private void SetParent(GameObject obj , Transform parent)
        {
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
        }

        public GameObject PlayParticle(GameObject prefab)
        {
            var obj = GetObject(prefab);

            FoundAndPlayParticle(obj);

            return obj;
        }
        public GameObject PlayParticle(GameObject prefab, Transform parent)
        {
            var obj = PlayParticle(prefab);
            SetParent(obj, parent);
            return obj;
        }


        private void FoundAndPlayParticle(GameObject obj)
        {
            var particleSystemCollection = obj.GetComponentsInChildren<ParticleSystem>();
            foreach (var particleSystem in particleSystemCollection)
            {
                particleSystem.Play();
            }
        }

        public void ReturnBackToPool(GameObject prefab, GameObject obj)
        {
            var prefabPoolParent = _prefabToPoolParent[prefab];
            obj.transform.SetParent(prefabPoolParent);
            obj.SetActive(false);
        }

        public class InitParameters
        {
            public ParticlePrefabSettings ParticlePoolSettings { get; set; }
        }

        [System.Serializable]
        public class ParticlePrefabSettings
        {
            [field: SerializeField] public GameObject[] ParticlePrefabCollection { get; private set; }
        }
    }
}