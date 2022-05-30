
using System;
using Cinemachine;
using DIC;
using GameCores;
using RG.Loader;
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
        [SerializeField] TutorialController _tutorialController;

        [SerializeField] GameObject[] _levelCollection;

        private GameObject FindAndDisableTapToPlay(bool state)
        {
            if (_tapToPlayGO != null)
            {
                _tapToPlayGO.SetActive(state);
                return _tapToPlayGO;
            }

            var mainMenuUIs = FindObjectsOfType<MainMenuUI>(true);
            if (mainMenuUIs == null || mainMenuUIs.Length == 0) { Debug.Log("MMUI couldn't found"); return null; }
            foreach (var mainMenuUI in mainMenuUIs)
            {
                var childCount = mainMenuUI.transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    var child = mainMenuUI.transform.GetChild(i);
                    var cchildCount = child.childCount;

                    for (int j = 0; j < cchildCount; j++)
                    {
                        var cchild = child.GetChild(j);
                        if (cchild.name == "TapToPlay")
                        {
                            Debug.Log("Found TapToPlay");
                            cchild.gameObject.SetActive(state);
                            return cchild.gameObject;
                        }
                    }
                }
            }

            return null;
        }

        GameObject _tapToPlayGO;
        private void Awake()
        {
            _genderSelectionController.Init();

            if (_genderSelectionController.IsGenderSelected)
            {
                RegisterToContainer();
            }
            else
            {
                _tapToPlayGO = FindAndDisableTapToPlay(false);
                _genderSelectionController.OnGenderSelected += RegisterToContainer;
            }
        }

        bool _showTutorial = true;

        public void RegisterToContainer()
        {
            _tapToPlayGO = FindAndDisableTapToPlay(true);

            var start = DateTime.Now;

            DIContainer.Instance.Register<IEventBus>(sortingOrder: -102)
                .RegisterConcreteType<EventBus>();
                //.AddDecorator<DebugEventBus>();

            DIContainer.Instance.RegisterSingle(_inputManager, sortingOrder: -100);

            GameObject levelPrefab = null;
            int levelIndex = RoosterHub.Central.GetLevelNo() - 1;

            Debug.Log("LevelIndex: " + levelIndex);

#if UNITY_EDITOR
            foreach (var prefab in _levelCollection)
            {
                var levelTestingSettings = prefab.GetComponent<LevelTestingSettings>();
                if (!levelTestingSettings.TestThisLevel) continue;

                levelPrefab = prefab;
                break;
            }

            if (levelIndex < 2)
            {
                levelPrefab = _tutorialController.GetTutorial(levelIndex);
            }
            else if (levelPrefab == null)
            {
                levelIndex = (levelIndex - 2) % _levelCollection.Length;
                levelPrefab = _levelCollection[levelIndex];
            }

#else
            if (levelIndex < 2)
            {
                levelPrefab = _tutorialController.GetTutorial(levelIndex);
            }
            else
            {
                levelIndex = (levelIndex - 2) % _levelCollection.Length;
                levelPrefab = _levelCollection[levelIndex];
            }
#endif

            var scenePrefabs = FindObjectsOfType<RoadSetter>();
            foreach (var setter in scenePrefabs)
            {
                setter.gameObject.SetActive(false);
            }

            DIContainer.Instance.RegisterSingle(_genderSelectionController.GameTypeGender, sortingOrder: -100);
            DIContainer.Instance.RegisterWhenInjectTo(_gameType, levelPrefab);
            DIContainer.Instance.RegisterSingle(_gameManager, sortingOrder: -100);

            DIContainer.Instance.RegisterSingle(_gameType, sortingOrder: -100);
            DIContainer.Instance.RegisterSingle(_roleSelector, sortingOrder: -100);

            // Buradan sonrasi degistirilecek
            DIContainer.Instance.RegisterSingle(_cameraToChangeFov, sortingOrder: -100);

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