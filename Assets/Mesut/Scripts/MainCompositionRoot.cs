
using System;
using Cinemachine;
using DIC;
using GameCores;
using UnityEngine;

namespace JR
{
    public class MainCompositionRoot : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] InputManager _inputManager;
        [SerializeField] GameType _gameType;
        [SerializeField] GenderSelectionController _genderSelectionController;
        [SerializeField] CinemachineVirtualCamera _cameraToChangeFov;
        [SerializeField] RoleSelector _roleSelector;
        [SerializeField] EndTrigger _endTrigger;

        [SerializeField] GameObject[] _levelCollection;

        private void Awake()
        {
            _genderSelectionController.Init();

            if (_genderSelectionController.IsGenderSelected)
            {
                RegisterToContainer();
            }
            else
                _genderSelectionController.OnGenderSelected += RegisterToContainer;
            //Init();
        }

        public void RegisterToContainer()
        {
            var start = DateTime.Now;

            //var assemblyInstanceCreator = new AssemblyInstanceCreator(typeof(MainCompositionRoot));
            var eventBus = new DebugEventBus(new EventBus());
            var coreEventBusCompositionRoot = new CoreEventBusCompositionRoot();


            DIContainer.Instance.RegisterSingle<IEventBus>(eventBus, sortingIndex: -100);
            //DIContainer.Instance.RegisterSingle(assemblyInstanceCreator, sortingIndex: -100);
            DIContainer.Instance.RegisterSingle(coreEventBusCompositionRoot, sortingIndex: -100);
            DIContainer.Instance.RegisterSingle(_inputManager, sortingIndex: -100);

#if UNITY_EDITOR
            GameObject levelPrefab = null;
            foreach (var prefab in _levelCollection)
            {
                var levelTestingSettings = prefab.GetComponent<LevelTestingSettings>();
                if (!levelTestingSettings.TestThisLevel) continue;

                levelPrefab = prefab;
            }

            if(levelPrefab == null)
            {
                var levelIndex = RoosterHub.Central.GetLevelNo() % _levelCollection.Length;
                levelPrefab = _levelCollection[levelIndex];
            }
#else
            var levelIndex = RoosterHub.Central.GetLevelNo() % _levelCollection.Length;
            var levelPefab = _levelCollection[levelIndex];
#endif

            var scenePrefabs = FindObjectsOfType<RoadSetter>();
            foreach (var setter in scenePrefabs)
            {
                setter.gameObject.SetActive(false);
            }

            DIContainer.Instance.RegisterSingle(_genderSelectionController.GameTypeGender, sortingIndex: -100);
            DIContainer.Instance.RegisterWhenInjectTo(_gameType, levelPrefab);
            DIContainer.Instance.RegisterSingle(_gameManager, sortingIndex: -100);

            DIContainer.Instance.RegisterSingle(_gameType, sortingIndex: -100);
            DIContainer.Instance.RegisterSingle(_roleSelector, sortingIndex: -100);

            // Buradan sonrasi degistirilecek
            DIContainer.Instance.RegisterSingle(_cameraToChangeFov, sortingIndex: -100);

            DIContainer.Instance.RegisterSingle(_endTrigger);

            _roleSelector.RegisterToContainer(_genderSelectionController.GameTypeGender);

            var compRootCollection = FindObjectsOfType<BaseCompRootGO>();
            for (int i = 0; i < compRootCollection.Length; i++)
                compRootCollection[i].RegisterToContainer();

            DIContainer.Instance.Resolve();

            var end = DateTime.Now;
            var diff = end.Subtract(start);

            Debug.Log("Seconds: " + diff.Seconds.ToString());
            Debug.Log("MiliSeconds: " + diff.Milliseconds.ToString());
        }
    }
}