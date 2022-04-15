using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RGLevel
{
    public class RunnerLevel : Level
    {
        [SerializeField] private LevelPath _levelPath;

        public LevelPath LevelPath
        {
            get { return _levelPath; }
        }
    }
}

