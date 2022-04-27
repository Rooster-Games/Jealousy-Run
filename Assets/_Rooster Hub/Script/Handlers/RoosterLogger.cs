using System.Collections;
using System.Collections.Generic;
using RG.Core;
using UnityEngine;

public static class RoosterLogger
{
    public static void Log(string text, Color color)
    {
        if(!GameSettings.Instance.showLogs) return;
        
        var x= ColorUtility.ToHtmlStringRGBA(color);
        Debug.Log("<color=#"+x+"> ? "+text+"</color>");
    }
    
}
