using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

namespace DI
{
    public class PMContainer
    {
        private static PMContainer _instance;
        public static PMContainer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PMContainer();

                return _instance;
            }
        }

        Dictionary<Type, object> _typeToInstance;
        Dictionary<Type, List<object>> _typeToInstanceForIniting;
        Dictionary<object, Dictionary<Type, object>> _whenInjectTo;

        private PMContainer()
        {
            _typeToInstance = new Dictionary<Type, object>();
            _typeToInstanceForIniting = new Dictionary<Type, List<object>>();
            _whenInjectTo = new Dictionary<object, Dictionary<Type, object>>();
        }

        public void RegisterSingle<T>(T instance)
        {
            var t = typeof(T);
            if (_typeToInstance.ContainsKey(t))
            {
                Debug.Log($"{t.Name} is already registered");
                return;
            }

            if(instance == null)
            {
                Debug.Log($"instance is null");
                return;
            }

            _typeToInstance.Add(t, instance);
        }

        public void RegisterForIniting<T>(T instance)
        {
            var t = typeof(T);

            if (!_typeToInstanceForIniting.ContainsKey(t))
                _typeToInstanceForIniting.Add(t, new List<object>());

            _typeToInstanceForIniting[t].Add(instance);
        }

        public void RegisterWhenInjecTo<T>(T instance, params object[] toObjectCollection)
        {
            var t = typeof(T);
            foreach (var toObject in toObjectCollection)
            {

                if (!_whenInjectTo.ContainsKey(toObject))
                {
                    _whenInjectTo.Add(toObject, new Dictionary<Type, object>());
                    _whenInjectTo[toObject].Add(t, instance);
                }
                else
                {
                    if (_whenInjectTo[toObject].ContainsKey(t))
                        return;

                    _whenInjectTo[toObject].Add(t, instance);
                }
            }
        }

        public void Solve()
        {
            foreach (var t in _typeToInstanceForIniting.Keys)
            {
                foreach (var objfi in _typeToInstanceForIniting[t])
                {
                    var objForIniting = objfi;

                    if (objForIniting == null)
                    {
                        Debug.Log($"{t.Name} typed instance is null");
                        continue;
                    }

                    var initInfo = t.GetMethod("Init");
                    var initParametersInfo = initInfo.GetParameters();

                    var initParameterType = initParametersInfo[0].ParameterType;
                    var initParametersObject = Activator.CreateInstance(initParameterType);
                    var fieldsInfo = initParameterType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (var fieldinfo in fieldsInfo)
                    {
                        var fieldType = fieldinfo.FieldType;
                        object obj = null;


                        if (_whenInjectTo.ContainsKey(objForIniting))
                        {
                            if (_whenInjectTo[objForIniting].ContainsKey(fieldType))
                                obj = _whenInjectTo[objForIniting][fieldType];
                        }

                        if (obj == null && _typeToInstance.ContainsKey(fieldType))
                            obj = _typeToInstance[fieldinfo.FieldType];


                        if (obj == null)
                        {
                            Debug.Log($"For {t.Name}: ");
                            Debug.Log($"{fieldType.Name} is not found");
                        }

                        fieldinfo.SetValue(initParametersObject, obj);
                    }

                    initInfo.Invoke(objForIniting, new[] { initParametersObject });
                }
            }
        }
    }
}
