using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using RG.Core;
using RG.Handlers;
using RG.Loader;

namespace RG.Looper
{
    public class SceneBasedLoop : MonoBehaviour, ILooper
    {
        public Level mainLevel;
        public List<Level> levelList = new List<Level>();


        #region ILevelLoader Interface

        public void LoadLevel()
        {
            StartCoroutine(LoadYourAsyncScene());
        }

        IEnumerator LoadYourAsyncScene()
        {
            var operation = SceneManager.LoadSceneAsync(levelList[GetLoopLevelNo()].levelScene);
           
            RoosterEventHandler.OnLoading?.Invoke();
            while (!operation.isDone)
            {
                yield return null;
            }
            
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

        public int GetRemoteLevelNo()
        {
            throw new NotImplementedException();
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