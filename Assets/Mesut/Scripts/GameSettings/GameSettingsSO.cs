using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JR
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "JR/GameSettings")]
    public class GameSettingsSO : ScriptableObject
    {
        [field: SerializeField] public BarChangingSettings BarChangingSettings { get; private set; }
        [field: SerializeField] public ParticlePool.ParticlePrefabSettings ParticlePrefabSettings { get; private set; }

    }
}