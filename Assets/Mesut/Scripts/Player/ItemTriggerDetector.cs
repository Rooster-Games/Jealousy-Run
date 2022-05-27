
using DG.Tweening;
using GameCores;
using UnityEngine;

namespace JR
{
    public class ItemTriggerDetector : MonoBehaviour
    {
        [SerializeField] OnCollectionSettings _collectionSettings;

        BarController _barController;

        float _onItemCollectionBarIncreaseValue;
        IEventBus _eventBus;
        ParticlePool _particlePool;

        public void Init(InitParameters initParameters)
        {
            _barController = initParameters.BarController;
            _onItemCollectionBarIncreaseValue = initParameters.BarChangingSettings.IncreaseSettings.WhileCollectingAnItemValue;
            _eventBus = initParameters.EventBus;
            _particlePool = initParameters.ParticlePool;
        }

        private void OnTriggerEnter(Collider other)
        {
            // var go = Instantiate(_collectionSettings.HeartExplosionPrefab);
            var go = _particlePool.PlayParticle(_collectionSettings.HeartExplosionPrefab);
            go.transform.position = other.transform.position;

            float timer = 0f;
            DOTween.To(() => timer, (x) => timer = x, 1f, 2f)
                .OnComplete(() => _particlePool.ReturnBackToPool(_collectionSettings.HeartExplosionPrefab, go));
            //Destroy(go, 2f);

            // other.gameObject.SetActive(false);
            other.transform.position = Vector3.zero;
            other.GetComponentInChildren<Renderer>().enabled = false;
            _barController.ChangeAmount(_onItemCollectionBarIncreaseValue);
            _eventBus.Fire<OnItemCollected>();
        }

        public class InitParameters
        {
            public BarController BarController { get; set; }
            public BarChangingSettings BarChangingSettings { get; set; }
            public IEventBus EventBus { get; set; }
            public ParticlePool ParticlePool { get; set; }
        }

        [System.Serializable]
        public class OnCollectionSettings
        {
            [SerializeField] GameObject _heartExplosionPrefab;

            public GameObject HeartExplosionPrefab => _heartExplosionPrefab;
        }
    }
}