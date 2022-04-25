
using GameCores;
using UnityEngine;

namespace JR
{
    public class MainCompositionRoot : MonoBehaviour
    {
        [SerializeField] GameManager _gameManager;
        [SerializeField] InputManager _inputManager;
        [SerializeField] PlayerCompositionRoot _playerCompositionRoot;
        [SerializeField] CrowdedControllerCompositionRoot _crowdedControllerCompositionRoot;
        [SerializeField] GameType _gameType;
        [SerializeField] BarCompositionSettings _barCompositionSettings;
        [SerializeField] GenderSelectionController _genderSelectionController;


        private void Awake()
        {
            _genderSelectionController.Init();

            if (_genderSelectionController.IsGenderSelected)
            {
                Init();
            }
            else
                _genderSelectionController.OnGenderSelected += Init;
            //Init();
        }

        public void Init()
        {
            var assemblyInstanceCreator = new AssemblyInstanceCreator(typeof(MainCompositionRoot));
            var eventBus = new EventBus();

            // eventBus composition root initing
            var coreEventBusCompositionRoot = new CoreEventBusCompositionRoot();
            var coreEventBusCompositionRootInitParameters = new CoreEventBusCompositionRoot.InitParameters();
            coreEventBusCompositionRootInitParameters.EventBus = eventBus;
            coreEventBusCompositionRootInitParameters.AssemblyInstanceCreator = assemblyInstanceCreator;

            coreEventBusCompositionRoot.Init(coreEventBusCompositionRootInitParameters);

            // input manager Initing
            var inputManagerInitParameters = new InputManager.InitParameters();
            inputManagerInitParameters.EventBus = eventBus;

            _inputManager.Init(inputManagerInitParameters);

            // Game Type Init
            var gameTypeInitParameters = new GameType.InitParameters();
            gameTypeInitParameters.ProtectorsGender = _genderSelectionController.GameTypeGender;
            _gameType.Init(gameTypeInitParameters);

            // player composition root init
            var pcrInitParameters = new PlayerCompositionRoot.InitParameters();
            pcrInitParameters.EventBus = eventBus;
            pcrInitParameters.InputManager = _inputManager;
            pcrInitParameters.GameType = _gameType;
            pcrInitParameters.BarController = _barCompositionSettings.BarController;

            _playerCompositionRoot.Init(pcrInitParameters);

            // crowded composition root init
            var ccrInitparameters = new CrowdedControllerCompositionRoot.InitParameters();
            ccrInitparameters.EventBus = eventBus;
            _crowdedControllerCompositionRoot.Init(ccrInitparameters);


            // game manager init
            var gmInitParameters = new GameManager.InitParameters();
            gmInitParameters.EventBus = eventBus;

            _gameManager.Init(gmInitParameters);

            // BarController Init
            var loveDataInitParameters = new LoveData.InitParameters();
            loveDataInitParameters.StartingValue = _barCompositionSettings.StartingPercent;
            _barCompositionSettings.LoveData.Init(loveDataInitParameters);

            var barInitParameters = new BaseBar.InitParameters();
            barInitParameters.StartingPercent = _barCompositionSettings.StartingPercent;
            _barCompositionSettings.Bar.Init(barInitParameters);

            var barControllerInitParameters = new BarController.InitParameters();
            barControllerInitParameters.Bar = _barCompositionSettings.Bar;
            barControllerInitParameters.LoveData = _barCompositionSettings.LoveData;
            _barCompositionSettings.BarController.Init(barControllerInitParameters);

        }
        //public void Init()
        //{
        //    // singletons
        //    var assemblyInstanceCreator = new AssemblyInstanceCreator(typeof(MainCompositionRoot));
        //    var eventBus = new EventBus();

        //    // register singletons

        //    PMContainer.Instance.RegisterSingle(eventBus);
        //    PMContainer.Instance.RegisterSingle(assemblyInstanceCreator);
        //    PMContainer.Instance.RegisterSingle(_inputManager);

        //    // for initing
        //    var coreEventBusCompositionRoot = new CoreEventBusCompositionRoot();

        //    // register for initing
        //    PMContainer.Instance.RegisterForIniting(_gameManager);
        //    PMContainer.Instance.RegisterForIniting(coreEventBusCompositionRoot);
        //    PMContainer.Instance.RegisterForIniting(_playerCompositionRoot);
        //    PMContainer.Instance.RegisterForIniting(_inputManager);

        //    _playerCompositionRoot.RegisterToContainer();
        //    _crowdedControllerCompositionRoot.RegisterToContainer();

        //    PMContainer.Instance.Resolve();
        //}
    }
}