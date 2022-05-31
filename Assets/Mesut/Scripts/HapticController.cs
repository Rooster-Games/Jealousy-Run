using System.Collections;
using System.Collections.Generic;
using GameCores;
using GameCores.CoreEvents;
using JR;
using UnityEngine;


public class HapticController : MonoBehaviour
{
    public enum HapticType { None, Success, Fail, Warning, Selection, Soft, Light, Medium, Hard }

    HapticExecuter _hapticExecuter;
    IEventBus _eventbus;

    public void Init(InitParameters initParameters)
    {
        _eventbus = initParameters.EventBus;

        _hapticExecuter = new HapticExecuter();

        _eventbus.Register<OnGameWin>((e) => Execute(HapticType.Success));
        // _eventbus.Register<OnGameFail>((e) => Execute(HapticType.Fail));
        _eventbus.Register<OnBarEmpty>((e) => Execute(HapticType.Fail));
        _eventbus.Register<OnItemCollected>((e) => Execute(HapticType.Soft));
        _eventbus.Register<OnSlap>((e) => Execute(HapticType.Medium));
    }


    private void Execute(HapticType hapticType)
    {
        _hapticExecuter.Execute(hapticType);
    }

    public class InitParameters
    {
        public IEventBus EventBus { get; set; }
    }

    private class TimeControl
    {
        bool _isExecuting;
        WaitForSeconds _waitSeconds;
        HapticExecuter _hapticExecuter;
        HapticType _hapticType;

        public bool IsExecuting => _isExecuting;

        public TimeControl(float timeToWait, HapticType hapticType, HapticExecuter hapticExecuter)
        {
            _waitSeconds = new WaitForSeconds(timeToWait);
            _hapticExecuter = hapticExecuter;
            _hapticType = hapticType;
        }

        public IEnumerator Execute()
        {
            _isExecuting = true;
            _hapticExecuter.Execute(_hapticType);
            yield return _waitSeconds;

            _isExecuting = false;
        }
    }

    private class HapticExecuter
    {
        int _onSlapCounter;
        int _onItemCounter;

        bool _failed;
        bool _win;

        public void Execute(HapticType type)
        {
            switch (type)
            {
                case HapticType.Success:
                    if(!_win)
                        RoosterHaptic.Success();
                    _win = true;
                    break;
                case HapticType.Fail:
                    if(!_failed)
                        RoosterHaptic.Fail();
                    _failed = true;
                    break;
                case HapticType.Selection:
                    RoosterHaptic.Selection();
                    break;
                case HapticType.Warning:
                    RoosterHaptic.Warning();
                    break;
                case HapticType.Soft:
                    {
                        if (_onItemCounter == 0)
                            RoosterHaptic.SoftImpact();

                        _onItemCounter++;
                        _onItemCounter = _onItemCounter % 3;
                        
                        break;
                    }
                case HapticType.Light:
                    RoosterHaptic.LightImpact();
                    break;
                case HapticType.Medium:
                    {
                        if(_onSlapCounter == 0)
                            RoosterHaptic.MediumImpact();
                        _onSlapCounter++;
                        _onSlapCounter = _onSlapCounter % 3; 
                        break;
                    }
                case HapticType.Hard:
                    RoosterHaptic.HardImpact();
                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    public class Settings
    {
        [field: SerializeField] public float OppositeGenderWaitForSeconds { get; private set; }
    }
}
