
using UnityEngine;

namespace JR
{
    public class ItemTriggerDetector : MonoBehaviour
    {
        [SerializeField] OnCollectionSettings _collectionSettings;

        BarController _barController;

        public void Init(InitParameters initParameters)
        {
            _barController = initParameters.BarController;
        }

        private void OnTriggerEnter(Collider other)
        {
            var itemCollectionData = other.GetComponent<ItemCollectionSettings>();

            var go = Instantiate(_collectionSettings.HeartExplosionPrefab);
            go.transform.position = other.transform.position;

            Destroy(go, 2f);

            other.gameObject.SetActive(false);
            _barController.ChangeAmount(itemCollectionData.BarIncreaseAmount);
        }

        public class InitParameters
        {
            public BarController BarController { get; set; }
        }

        [System.Serializable]
        public class OnCollectionSettings
        {
            [SerializeField] GameObject _heartExplosionPrefab;

            public GameObject HeartExplosionPrefab => _heartExplosionPrefab;
        }
    }
}