using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JR
{
    public class SingleControllerCompositionRoot : MonoBehaviour
    {
        public void Init(InitParameters initParameters)
        {
            // self
            var singleController = GetComponent<SingleController>();

            // child
            var animator = GetComponentInChildren<Animator>();
            var triggerDetector = GetComponentInChildren<SlapEnemyDetector>();

            var animatorController = new AnimatorControllerFactory().Create(animator);

            // animation evetns init

            var pAEInitParameters = new PlayerAnimationEvents.InitParameters();
            pAEInitParameters.AnimatorController = animatorController;
            var playerAnimationEvents = GetComponentInChildren<PlayerAnimationEvents>();
            playerAnimationEvents.Init(pAEInitParameters);

            // singlecontroller init
            var singleControllerInitParameters = new SingleController.InitParameters();
            singleControllerInitParameters.AnimatorController = animatorController;
            singleControllerInitParameters.MoveSettings = initParameters.MoveSettings;
            singleControllerInitParameters.ExhaustCheckerSettings = initParameters.ExhaustCheckerSettings;
            singleControllerInitParameters.AnimationEvents = playerAnimationEvents;

            singleController.Init(singleControllerInitParameters);

            // trigger detector init
            if (triggerDetector != null)
            {
                var triggerDetectorInitParameters = new SlapEnemyDetector.InitParameters();
                triggerDetectorInitParameters.SingleController = singleController;

                triggerDetector.Init(triggerDetectorInitParameters);
            }

            animator.runtimeAnimatorController = initParameters.RuntimeAnimatorController;

        }

        public class InitParameters
        {
            public GameType GameType { get; set; }
            public DoTweenSwapper.MoveSettings MoveSettings { get; set; }
            public ExhaustChecker.Settings ExhaustCheckerSettings { get; set; }
            public RuntimeAnimatorController RuntimeAnimatorController { get; set; }
        }
    }
}