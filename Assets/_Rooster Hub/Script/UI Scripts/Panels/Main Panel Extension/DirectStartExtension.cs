using System;
using System.Collections;
using System.Collections.Generic;
using RG.Loader;
using UnityEngine;

public class DirectStartExtension : MonoBehaviour,IExtension
{
    public void RunExtension()
    {
        var uiLoop = FindObjectOfType<UI_Loop>();
        if (uiLoop._uiIndex!=0)
        {
            GetComponent<MainMenuUI>().OnClickStartGameButton();
        }
    }
}