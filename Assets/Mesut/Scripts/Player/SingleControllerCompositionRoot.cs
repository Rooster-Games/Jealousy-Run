using System.Collections;
using System.Collections.Generic;
using DIC;
using GameCores;
using TMPro;
using UnityEngine;

namespace JR
{
    public class SingleControllerCompositionRoot : BaseCompRootGO
    {
        public override void RegisterToContainer()
        {
            DIContainer.Instance.RegisterGameObject(gameObject)
                .RegisterComponent<GenderInfo>()
                .RegisterComponent<SingleController>()
                .RegisterComponent<Animator>()
                .RegisterComponent<PlayerAnimationEvents>()
                .RegisterComponent<SlapEnemyDetector>(true)
                .RegisterComponent<PushEnemyDetector>(true)
                .RegisterComponent<ItemTriggerDetector>(true)
                .RegisterFactoryFor<IAnimatorController, AnimatorControllerFactory>()
                .RegisterWhenInjectTo<ExhaustChecker, IAnimatorController>()
                .RegisterWithCheck<ExhaustChecker, IAnimatorController, ProtectorChecker>(new ProtectorChecker());
        }
    }

    public interface IChecker
    {
        bool Check();
    }

    public class ProtectorChecker : IChecker
    {
        GameType _gameType;
        GenderInfo _genderInfo;

        public void Init(InitParameters initParameters)
        {
            _gameType = initParameters.GameType;
            _genderInfo = initParameters.GenderInfo;
        }

        public bool Check()
        {
            return _gameType.ProtectorGender == _genderInfo.Gender;
        }


        public class InitParameters 
        {
            public GenderInfo GenderInfo { get; set; }
            public GameType GameType { get; set; }
        }
    }

}