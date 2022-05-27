

using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System.Linq;
using DIC;
using GameCores;

namespace JR
{
    public class PlayerCompositionRoot : BaseCompRootGO
    {
        [SerializeField] PlayerSettingsSO _playerSettings;
        [SerializeField] CoroutineSwapper _coroutineSwapper;

        public override void RegisterToContainer()
        {
            DIContainer.Instance.RegisterGameObject(gameObject)
                .RegisterComponent<CinemachineDollyCart>()
                .RegisterComponent<DollyCartController>()
                .RegisterComponent<PlayerController>()
                .RegisterComponent<DynamicBoneActivator>()
                .RegisterComponent<AnimatorActivator>()
                .RegisterWhenInjectTo<SpeedChanger, DollyCartController>();
        }
    }
}