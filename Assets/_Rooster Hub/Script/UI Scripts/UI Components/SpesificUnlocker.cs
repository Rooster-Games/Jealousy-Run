using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnlockMode
{
    PerLevel,
    ViaList
}

public class SpesificUnlocker : MonoBehaviour, IExtension
{
    public GameObject unlockableObject;
    public UnlockMode _unlockMode;
    public int unlockPerLevel;
    public int[] unlockQueue;

    public void RunExtension()
    {
        switch (_unlockMode)
        {
            case UnlockMode.PerLevel:
                CheckPerLevel();
                break;
            case UnlockMode.ViaList:
                CheckViaList();
                break;
            default:
                break;
        }
    }

    private void CheckPerLevel()
    {
        if (RoosterHub.Central.GetLevelNo() % unlockPerLevel == 0)
        {
            Debug.LogError("UNLOCKED");
            unlockableObject.SetActive(true);
        }
        else
        {
            Debug.LogError("LOCKED");
            unlockableObject.SetActive(false);
        }
    }

    private void CheckViaList()
    {
    }
}