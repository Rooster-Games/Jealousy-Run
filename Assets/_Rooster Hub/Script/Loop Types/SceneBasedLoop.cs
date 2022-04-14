using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using RG.Core;
using RG.Loader;

namespace RG.Looper
{
    public class SceneBasedLoop : MonoBehaviour, ILooper
    {
        public Level mainLevel;
        public List<Level> levelList = new List<Level>();
        public UI_Loop loop ;

        #region ILevelLoader Interface

        public void LoadLevel()
        {
            SceneManager.LoadScene(levelList[GetLoopLevelNo()].levelScene);
        }
      
        public void LoadMainMenu()
        {
            SceneManager.LoadScene(mainLevel.levelScene);
        }

        public int GetLoopLevelNo()
        {
            var sceneNo = GamePrefs.LevelNo % levelList.Count;
            return sceneNo;
        }

        #endregion
    }
}

[Serializable]
public struct Level
{
    [Scene] public string levelScene;
    // public string levelName;
    // public string levelDescription;
}