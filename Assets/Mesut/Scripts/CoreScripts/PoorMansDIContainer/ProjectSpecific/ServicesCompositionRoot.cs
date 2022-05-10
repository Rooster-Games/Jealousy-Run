using System.Collections;
using System.Collections.Generic;
using DIC;
using GameCores;
using JR;
using UnityEngine;

public class ServicesCompositionRoot : BaseCompRootGO
{
    [SerializeField] CoroutineSwapper _swapper;

    public override void RegisterToContainer()
    {
        DIContainer.Instance.RegisterSingle<ISwapper>(_swapper);
        DIContainer.Instance.RegisterSingle(new CameraFovChanger());
        DIContainer.Instance.RegisterSingle(new ExhaustChecker());
        DIContainer.Instance.RegisterSingle(new SpeedChanger());
    }
}