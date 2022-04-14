using System.Collections;
using System.Collections.Generic;
using ElephantSDK;
using UnityEngine;

public class RoosterAnalyticSender
{
    public static void SendStartEvent()
    {
        RoosterLogger.Log("LEVEL : "+RoosterHub.Central.GetLevelNo()+" Started ",Color.yellow);
        Elephant.LevelStarted(RoosterHub.Central.GetLevelNo());
    }

    public static void SendWinEvent()
    {
        RoosterLogger.Log("LEVEL : "+RoosterHub.Central.GetLevelNo()+" Win ",Color.green);
        Elephant.LevelCompleted(RoosterHub.Central.GetLevelNo());
    }

    public static void SendFailEvent()
    { 
        RoosterLogger.Log("LEVEL : "+RoosterHub.Central.GetLevelNo()+" Fail ",Color.red);
        Elephant.LevelFailed(RoosterHub.Central.GetLevelNo());
    }

    public static void SendRestartEvent()
    {
        RoosterLogger.Log("LEVEL : "+RoosterHub.Central.GetLevelNo()+" Restart ",Color.magenta);
        Elephant.Event("Restart", RoosterHub.Central.GetLevelNo());
    }

    public static void SendSkipEvent()
    {
        RoosterLogger.Log("LEVEL : "+RoosterHub.Central.GetLevelNo()+" Skipped ",Color.cyan);
        Elephant.Event("SkipLevel", RoosterHub.Central.GetLevelNo());
    }

}
