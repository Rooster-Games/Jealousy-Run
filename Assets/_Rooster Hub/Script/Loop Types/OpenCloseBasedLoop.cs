using System.Collections.Generic;
using ElephantSDK;
using NaughtyAttributes;
using RG.Core;
using RG.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenCloseBasedLoop : MonoBehaviour, ILooper
{
    public Transform levelParentObject;

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
            if (useRemote)
            {
                var sortedArray = Remote.Instance.GetRemoteLevelArray().Split(char.Parse(","));
                foreach (var item in sortedArray)
                {
                    sortedLevels.Add(int.Parse(item));
                }
            }

            CloseAllLevels();
            LoadLevel();
        }
    }

    void CloseAllLevels()
    {
        for (int i = 0; i < levelParentObject.childCount; i++)
        {
            levelParentObject.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void LoadLevel()
    {
        if (useRemote)
        {
            levelParentObject.GetChild(sortedLevels[GetRemoteLevelNo()]).gameObject.SetActive(true);
        }
        else
        {
            levelParentObject.GetChild(GetLoopLevelNo()).gameObject.SetActive(true);
        }
    }

    public void LoadMainMenu()
    {
    }

    public int GetLoopLevelNo()
    {
        var levelNo = GamePrefs.LevelNo % levelParentObject.childCount;
        return levelNo;
    }

    public int GetRemoteLevelNo()
    {
        var levelNo = GamePrefs.LevelNo % sortedLevels.Count;
        return levelNo;
    }
}