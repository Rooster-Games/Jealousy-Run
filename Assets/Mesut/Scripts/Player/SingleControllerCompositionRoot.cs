using System.Collections;
using System.Collections.Generic;
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

            // singlecontroller init
            var singleControllerInitParameters = new SingleController.InitParameters();
            singleControllerInitParameters.AnimatorController = animatorController;
            singleControllerInitParameters.MoveSettings = initParameters.MoveSettings;

            singleController.Init(singleControllerInitParameters);

            // trigger detector init
            if (triggerDetector != null)
            {
                var triggerDetectorInitParameters = new SlapEnemyDetector.InitParameters();
                triggerDetectorInitParameters.SingleController = singleController;

                triggerDetector.Init(triggerDetectorInitParameters);
            }
        }

        public class InitParameters
        {
            public GameType GameType { get; set; }
            public DoTweenSwapper.MoveSettings MoveSettings { get; set; }
        }
    }
}