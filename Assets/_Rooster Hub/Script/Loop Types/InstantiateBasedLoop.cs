using System;
using System.Collections.Generic;
using ElephantSDK;
using NaughtyAttributes;
using RG.Core;
using RG.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiateBasedLoop : MonoBehaviour, ILooper
{
    public List<GameObject> levels = new List<GameObject>();
    public bool developmentMode;

    public bool useRemote;
    [ShowIf("useRemote")] public List<int> sortedLevels;


    private void Awake()
    {
        

        if (developmentMode && !RoosterEventHandler.isDevelopmentMode)
        {
            SceneManager.LoadScene(1);
            RoosterEventHandler.isDevelopmentMode = true;
        }
        else
        {
            LoadLevel();
        }
        
        if (useRemote)
        {
            var sortedArray = Remote.Instance.GetRemoteLevelArray().Split(char.Parse(","));
            foreach (var item in sortedArray)
            {
                sortedLevels.Add(int.Parse(item));
            }
        }
    }

    public void LoadLevel()
    {
        if (useRemote)
        {
            Instantiate(levels[sortedLevels[GetRemoteLevelNo()]]);
        }
        else
        {
            Instantiate(levels[GetLoopLevelNo()]);
        }
    }

    public void LoadMainMenu()
    {
        throw new NotImplementedException();
    }

    public int GetLoopLevelNo()
    {
        var levelNo = GamePrefs.LevelNo % levels.Count;
        return levelNo;
    }

    public int GetRemoteLevelNo()
    {
        var levelNo = GamePrefs.LevelNo % sortedLevels.Count;
        return levelNo;
    }
}