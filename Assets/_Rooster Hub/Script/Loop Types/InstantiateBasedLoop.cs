using System;
using System.Collections.Generic;
using RG.Core;
using UnityEngine;

public class InstantiateBasedLoop : MonoBehaviour,ILooper
{
    public List<GameObject> levels = new List<GameObject>();

    private void Awake()
    {
        LoadLevel();
    }
    public void LoadLevel()
    {
        Instantiate(levels[GetLoopLevelNo()]);
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

}
