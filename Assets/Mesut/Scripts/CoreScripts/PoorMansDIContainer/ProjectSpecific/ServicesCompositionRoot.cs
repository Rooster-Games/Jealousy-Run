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
        DIContainer.Instance.Register<CameraFovChanger>();
        DIContainer.Instance.Register<ExhaustChecker>();
        DIContainer.Instance.Register<SpeedChanger>();
        DIContainer.Instance.Register<ParticlePool>();
    }
}