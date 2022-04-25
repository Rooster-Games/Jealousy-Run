
using UnityEngine;

namespace JR
{
    public class ItemTriggerDetector : MonoBehaviour
    {
        BarController _barController;

        public void Init(InitParameters initParameters)
        {
            _barController = initParameters.BarController;
        }

        private void OnTriggerEnter(Collider other)
        {
            var itemCollectionData = other.GetComponent<ItemCollectionSettings>();

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