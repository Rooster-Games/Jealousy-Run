using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System;
using System.Reflection;

public static class DebugExtensions
{
    public static void CheckNullField<T>(this T instance, [CallerMemberName] string callerName = "")
    {
        bool isShowedInfo = false;

        string debugMessage = "";

        Type type = typeof(T);
        var fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);


        foreach (var fieldInfo in fieldInfos)
        {
            var value = fieldInfo.GetValue(instance);

            if (value == null)
            {
                if (!isShowedInfo)
                    ShowMainInfo();

                debugMessage += $"   - FieldName: {fieldInfo.Name}\n";
            }
        }

        if (isShowedInfo)
            Debug.LogError(debugMessage);

        void ShowMainInfo()
        {
            debugMessage = $"=== FieldOwnerType: {type.Name} ===\n";

            isShowedInfo = true;
        }
    }

    public static void SelfDebug<T>(this T instance, string message)
    {
        Debug.Log($"{message}: {instance}");
    }
}