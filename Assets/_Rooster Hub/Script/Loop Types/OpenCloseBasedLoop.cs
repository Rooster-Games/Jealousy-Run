using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using RG.Core;
using UnityEngine;

public class OpenCloseBasedLoop : MonoBehaviour,ILooper
{
    public Transform levelParentObject;
    private void Awake()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        
        levelParentObject.GetChild(GetLoopLevelNo()).gameObject.SetActive(true);
    }

    public void LoadMainMenu()
    {
        throw new System.NotImplementedException();
    }

    public int GetLoopLevelNo()
    {
        var levelNo = GamePrefs.LevelNo % levelParentObject.childCount;
        return levelNo;
    }
}
