using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Linq;

namespace GameCores
{
    public class DebugEventBus : IEventBus
    {
        private string INFO_DATA_PATH = Application.dataPath + "/Mesut/Scripts/CoreScripts/Info";
        private string INFO_OBJ_PATH = "Assets/Mesut/Scripts/CoreScripts/Info/EventBusInfo.asset";
        IEventBus _eventBus;

        EventBusInfoSO _infoSO;

        public DebugEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
            CreateInfo();
        }

        public void Register<T>(Action<T> action) where T : IEventData
        {

            var targetStr = action.Target.ToString();
            string className = targetStr;
            if (targetStr.Contains("("))
            {
                var startIndex = targetStr.IndexOf('(') + 1;
                var endIndex = targetStr.IndexOf(')');
                className = targetStr.Substring(startIndex, endIndex - startIndex);
            }
            
            // UnityEngine.Debug.Log(className);

            var methodName = action.Method.Name;
            // UnityEngine.Debug.Log("Action Method Name: " + action.Method.Name);

            _infoSO.AddRegisterInfo(typeof(T).Name, className, methodName);

            _eventBus.Register(action);
        }

        public void Fire<T>(T data = default) where T : IEventData
        {
            _infoSO.AddFireInfo(typeof(T).Name, NameOfCallingClass());

            _eventBus.Fire(data);
        }

        public void Raise<T>() where T : IEventData, new()
        {
            _eventBus.Raise<T>();
        }

        public T GetData<T>() where T : IEventData, new()
        {
            return _eventBus.GetData<T>();
        }

        public void UnRegister<T>(Action<T> action) where T : IEventData
        {
            _eventBus.UnRegister(action);
        }

        private void CreateInfo()
        {
            if (!Directory.Exists(INFO_DATA_PATH))
                Directory.CreateDirectory(INFO_DATA_PATH);

            _infoSO = (EventBusInfoSO)AssetDatabase.LoadAssetAtPath(INFO_OBJ_PATH, typeof(EventBusInfoSO));

            if(_infoSO == null)
            {
                _infoSO = ScriptableObject.CreateInstance<EventBusInfoSO>();

                AssetDatabase.CreateAsset(_infoSO, INFO_OBJ_PATH);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                _infoSO.Init();
            }

            var assembly = typeof(IEventData).Assembly;
            var eventNames = assembly.GetTypes().Where(x => (x.IsClass || x.IsValueType) && typeof(IEventData).IsAssignableFrom(x)).Select(x => x.Name);

            foreach (var eventName in eventNames)
            {
                _infoSO.AddEventInfo(eventName);
            }
        }

        public string NameOfCallingClass()
        {
            string fullName;
            Type declaringType;
            int skipFrames = 2;
            do
            {
                MethodBase method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    return method.Name;
                }
                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return fullName;
        }
    }
}
