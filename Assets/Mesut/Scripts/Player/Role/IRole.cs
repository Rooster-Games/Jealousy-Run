using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    public interface IRole
    {
        void OnClick();
        void OnRelease();
    }

    public class ProtectorRole : IRole
    {
        SlapEnemyDetector _slapDetector;
        ISwapper _positionSwapper;
        ExhaustChecker _exhaustChecker;
        IAnimatorController _animatorController;
        BarController _barController;

        public void Init(InitParameters initParameters)
        {
            _slapDetector = initParameters.SlapEnemyDetector;
            _positionSwapper = initParameters.PositionSwapper;
            _exhaustChecker = initParameters.ExhaustChecker;
            _animatorController = initParameters.AnimatorController;
            _barController = initParameters.BarController;
        }

        public void OnClick()
        {
            _slapDetector.gameObject.SetActive(true);
            _positionSwapper.Swap();
            _exhaustChecker.StartTimer();
            _animatorController.SetTrigger("turnLeft");
        }

        public void OnRelease()
        {
        }

        public class InitParameters
        {
            public SlapEnemyDetector SlapEnemyDetector { get; set; }
            public ISwapper PositionSwapper { get; set; }
            public ExhaustChecker ExhaustChecker { get; set; }
            public IAnimatorController AnimatorController { get; set; }
            public BarController BarController { get; set; }
        }
    }

    public class DefendentRole : IRole
    {
        public void OnClick()
        {
            throw new System.NotImplementedException();
        }

        public void OnRelease()
        {
            throw new System.NotImplementedException();
        }
    }
}