using System.Collections;
using System.Collections.Generic;
using DIC;
using JR;
using UnityEngine;

public class TestControllerCompositionRoot : MonoBehaviour
{
    public void RegisterToContainer()
    {
        DIContainer.Instance.RegisterGameObject(gameObject)
            .RegisterComponent<TestController>()
            .RegisterComponent<Animator>();
            //.RegisterFactoryFor<IAnimatorController, AnimatorControllerFactory>();
    }
}
