using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DIContainer
{
    public class DIContainer : MonoBehaviour
    {
        private static DIContainer _instance;
        public static DIContainer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DIContainer();

                return _instance;
            }
        }

        List<object> _forInitingObjectList;
        Dictionary<object, Dictionary<Type, object>> _whenInjectToMap;
        Dictionary<Type, object> _registeredTypeToInstance;

        DIContainer()
        {
            _forInitingObjectList = new List<object>();
            _whenInjectToMap = new Dictionary<object, Dictionary<Type, object>>();
            _registeredTypeToInstance = new Dictionary<Type, object>();
        }

        public void RegisterSingle<T>(T instance)
        {
            instance.CheckForIniting();
            var t = typeof(T);
            if(_registeredTypeToInstance.ContainsKey(t))
            {
                Debug.Log($"=== {t.Name} Has Been Already Registered As Single. ===");
            }

            _registeredTypeToInstance.Add(t, instance);
        }

        public void RegisterWhenInjectTo(object dependent, object dependency)
        {
            var dependencyType = dependency.GetType();
            if (!_whenInjectToMap.ContainsKey(dependent))
                _whenInjectToMap.Add(dependent, new Dictionary<Type, object>());

            if (_whenInjectToMap[dependent].ContainsKey(dependencyType))
                Debug.Log($"=== Dependency has already get their when inject to type of {dependencyType.Name} ===");

            _whenInjectToMap[dependent].Add(dependencyType, dependency);
        }

        public void AddInstanceForIniting<T>(T instance)
        {
            if (instance == null)
                throw new System.Exception($"=== Instance of {nameof(T)} Type is Null cannot Register for initing ===");

            if (_forInitingObjectList.Contains(instance))
                throw new Exception($"=== Trying to add same Instance of {nameof(T)} type for initing ===");

            _forInitingObjectList.Add(instance);
        }

        public void Reset()
        {
            _instance = new DIContainer();
        }

        public void Resolve()
        {
            foreach (var instanceForIniting in _forInitingObjectList)
            {
                var t = instanceForIniting.GetType();

                var methodInfo = t.GetMethod("Init");
                var initParametersInfos = methodInfo.GetParameters();
                var initParametersType = initParametersInfos[0].ParameterType;

                var initParameterInstance = Activator.CreateInstance(initParametersType);

                var fieldsInfo = initParametersType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var fieldInfo in fieldsInfo)
                {
                    var fieldType = fieldInfo.FieldType;

                    object fieldInstance = null;

                    if(_whenInjectToMap.TryGetValue(instanceForIniting, out var typeInstanceMap))
                        if(typeInstanceMap.TryGetValue(fieldType, out object whenInjectToInstance))
                            fieldInstance = whenInjectToInstance;

                    if (fieldInstance == null && _registeredTypeToInstance.TryGetValue(fieldType, out object registeredInstance))
                        fieldInstance = registeredInstance;

                    if(fieldInstance == null)
                    {
                        Debug.Log($"For {t.Name}: ");
                        Debug.Log($"{fieldType.Name} is not found");
                    }

                    fieldInfo.SetValue(initParameterInstance, fieldInstance);
                }

                methodInfo.Invoke(instanceForIniting, new object[] { initParameterInstance });
            }
        }
    }

    public static class DIExtensions
    {
        public static void CheckForIniting<T>(this T instance)
        {
            var t = typeof(T);
            var methodInfo = t.GetMethod("Init");

            if (methodInfo != null)
                DIContainer.Instance.AddInstanceForIniting(instance);
        }
    }
}