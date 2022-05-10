
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
            var assemblyInstanceCreator = new AssemblyInstanceCreator(typeof(MainCompositionRoot));
            var eventBus = new DebugEventBus(new EventBus());
            var coreEventBusCompositionRoot = new CoreEventBusCompositionRoot();


            DIContainer.Instance.RegisterSingle<IEventBus>(eventBus, sortingIndex: -100);
            DIContainer.Instance.RegisterSingle(assemblyInstanceCreator, sortingIndex: -100);
            DIContainer.Instance.RegisterSingle(coreEventBusCompositionRoot, sortingIndex: -100);
            DIContainer.Instance.RegisterSingle(_inputManager, sortingIndex: -100);


            var levelIndex = RoosterHub.Central.GetLevelNo() % _levelCollection.Length;
            var levelPefab = _levelCollection[levelIndex];

            DIContainer.Instance.RegisterSingle(_genderSelectionController.GameTypeGender, sortingIndex: -100);
            DIContainer.Instance.RegisterWhenInjectTo(_gameType, levelPefab);
            DIContainer.Instance.RegisterSingle(_gameManager, sortingIndex: -100);

            DIContainer.Instance.RegisterSingle(_gameType, sortingIndex: -100);
            DIContainer.Instance.RegisterSingle(_roleSelector, sortingIndex: -100);

            // Buradan sonrasi degistirilecek
            DIContainer.Instance.RegisterSingle(_cameraToChangeFov, sortingIndex: -100);

            _roleSelector.RegisterToContainer(_genderSelectionController.GameTypeGender);

            var compRootCollection = FindObjectsOfType<BaseCompRootGO>();
            for (int i = 0; i < compRootCollection.Length; i++)
                compRootCollection[i].RegisterToContainer();

            DIContainer.Instance.Resolve();
        }
    }
}