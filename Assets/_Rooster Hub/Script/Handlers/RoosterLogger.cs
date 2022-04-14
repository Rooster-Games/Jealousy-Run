using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoosterLogger
{
    public static void Log(string text, Color color)
    {
        var x= ColorUtility.ToHtmlStringRGBA(color);
        Debug.Log("<color=#"+x+"> ? "+text+"</color>");
    }
    
}
