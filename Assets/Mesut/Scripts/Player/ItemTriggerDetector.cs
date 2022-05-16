
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
        public void Init(InitParameters initParameters)
        {
            _barController = initParameters.BarController;
            _onItemCollectionBarIncreaseValue = initParameters.BarChangingSettings.IncreaseSettings.WhileCollectingAnItemValue;
            _eventBus = initParameters.EventBus;
        }

        private void OnTriggerEnter(Collider other)
        {
            var go = Instantiate(_collectionSettings.HeartExplosionPrefab);
            go.transform.position = other.transform.position;

            Destroy(go, 2f);

            other.gameObject.SetActive(false);
            _barController.ChangeAmount(_onItemCollectionBarIncreaseValue);
            _eventBus.Fire<OnItemCollected>();
        }

        public class InitParameters
        {
            public BarController BarController { get; set; }
            public BarChangingSettings BarChangingSettings { get; set; }
            public IEventBus EventBus { get; set; }
        }

        [System.Serializable]
        public class OnCollectionSettings
        {
            [SerializeField] GameObject _heartExplosionPrefab;

            public GameObject HeartExplosionPrefab => _heartExplosionPrefab;
        }
    }
}