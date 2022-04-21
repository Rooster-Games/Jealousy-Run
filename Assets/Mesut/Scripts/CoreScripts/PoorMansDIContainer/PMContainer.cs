using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.CompilerServices;
using Cinemachine;

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

        List<GameObjectRegistrationData> _goRegistrationDataList;

        private PMContainer()
        {
            _typeToInstance = new Dictionary<Type, object>();
            _typeToInstanceForIniting = new Dictionary<Type, List<object>>();
            _whenInjectTo = new Dictionary<object, Dictionary<Type, object>>();
            _goRegistrationDataList = new List<GameObjectRegistrationData>();
        }

        public static void Reset()
        {
            _instance = new PMContainer();
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

        public void RegisterForIniting(Type t, object instance)
        {
            if (!_typeToInstanceForIniting.ContainsKey(t))
                _typeToInstanceForIniting.Add(t, new List<object>());

            _typeToInstanceForIniting[t].Add(instance);
        }

        public void RegisterForIniting<T>(T instance)
        {
            var t = typeof(T);
            RegisterForIniting(t, instance);
        }

        public void RegisterWhenInjecTo(Type t, object instance, params object[] toObjectCollection )
        {
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

        public void RegisterWhenInjecTo<T>(T instance, params object[] toObjectCollection)
        {
            var t = typeof(T);
            RegisterWhenInjecTo(t, instance, toObjectCollection);
        }

        public GameObjectRegistrationData RegisterGameobject(GameObject gameObject)
        {
            var registData = new GameObjectRegistrationData(gameObject);
            _goRegistrationDataList.Add(registData);
            return registData;
        }

        public void Resolve()
        {
            foreach (var goRegistrationData in _goRegistrationDataList)
            {
                goRegistrationData.RegisterToContainer();
            }


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

                    if(initInfo == null)
                    {
                        Debug.Log($"=== {t.Name} has no Init Method ===");
                        continue;
                    }

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


        // component register et, register ederken sadece generic type kullanilacak
        // componentin instance'i referanslamak icin, GetComponent'i once selften basla,
        // sonra child olarak git,
        // eger init ihtiyaci varsa - init olarak ayir,
        // component eger birden fazla ise, o zaman spesifik game object'e gore ayarlanmasi gerekiyor
        
        public class GameObjectRegistrationData
        {
            GameObject _gameObject;

            Dictionary<GameObject, Dictionary<Type, object>> _gameObjectToComponentMap; 
            Dictionary<Type, List<Component>> _typeToComponentInstanceMap;

            Dictionary<Type, List<(GameObject, object)>> _typeToForInitingMap;

            public GameObjectRegistrationData(GameObject gameObject)
            {
                _gameObject = gameObject;

                _typeToForInitingMap = new Dictionary<Type, List<(GameObject, object)>>();
                _typeToComponentInstanceMap = new Dictionary<Type, List<Component>>();
                _gameObjectToComponentMap = new Dictionary<GameObject, Dictionary<Type, object>>();
            }

            public GameObjectRegistrationData RegisterComponent<T>() where T: Component
            {
                var t = typeof(T);
                Debug.Log(t.Name);

                var component = _gameObject.GetComponentInChildren<T>();

                RegisterSingleComponent(component);

                return this;
            }

            public GameObjectRegistrationData RegisterComponentCollection<T>() where T: Component
            {
                var componentCollection = _gameObject.GetComponentsInChildren<T>();

                foreach (var component in componentCollection)
                {
                    RegisterSingleComponent(component);
                }

                return this;
            }

            private void RegisterSingleComponent<T>(T component) where T : Component
            {
                if (component != null)
                {
                    CheckForIniting(component);

                    var componentGO = component.gameObject;

                    if (!_gameObjectToComponentMap.ContainsKey(componentGO))
                    {
                        _gameObjectToComponentMap.Add(componentGO, new Dictionary<Type, object>());
                    }

                    var t = typeof(T);

                    _gameObjectToComponentMap[componentGO].Add(t, component);

                    if (!_typeToComponentInstanceMap.ContainsKey(t))
                        _typeToComponentInstanceMap.Add(t, new List<Component>());

                    _typeToComponentInstanceMap[t].Add(component);
                }

                if(component == null)
                {
                    Debug.Log($"Type of {typeof(T).Name} is null");
                }
            }

            public void RegisterToContainer()
            {
                foreach (var t in _typeToForInitingMap.Keys)
                {
                    var dataList = _typeToForInitingMap[t];

                    var initInfo = t.GetMethod("Init");
                    var initParametersInfo = initInfo.GetParameters();

                    var initParameterType = initParametersInfo[0].ParameterType;
                    var initParametersObject = Activator.CreateInstance(initParameterType);
                    var fieldsInfo = initParameterType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (var fieldinfo in fieldsInfo)
                    {
                        var fieldType = fieldinfo.FieldType;

                        Debug.Log($"Init Field Type: {fieldType.Name}");

                        foreach (var data in dataList)
                        {
                            var go = data.Item1;
                            if (_gameObjectToComponentMap.TryGetValue(go, out var typeToComponentMap))
                            {
                                if (typeToComponentMap.TryGetValue(fieldType, out var selfComponent))
                                {
                                    PMContainer.Instance.RegisterWhenInjecTo(fieldType, selfComponent, data.Item2);
                                }
                                else
                                {
                                    var parentT = go.transform.parent;
                                    bool isSetted = false;
                                    while(parentT != null)
                                    {
                                        if (_gameObjectToComponentMap.TryGetValue(parentT.gameObject, out var parentTypeToComponentMap))
                                        {
                                            if (parentTypeToComponentMap.TryGetValue(fieldType, out var parentComponent))
                                            {
                                                PMContainer.Instance.RegisterWhenInjecTo(fieldType, parentComponent, data.Item2);
                                                isSetted = true;
                                                break;
                                            }
                                        }

                                        parentT = parentT.parent;
                                    }

                                    if (!isSetted)
                                    {
                                        if (_typeToComponentInstanceMap.TryGetValue(fieldType, out var componentList))
                                        {
                                            PMContainer.Instance.RegisterWhenInjecTo(fieldType, componentList[0], data.Item2);
                                        }
                                    }
                                }
                            }
                            else if (_typeToComponentInstanceMap.TryGetValue(fieldType, out var componentList))
                            {
                                PMContainer.Instance.RegisterWhenInjecTo(fieldType, componentList[0], data.Item2);
                            }


                            PMContainer.Instance.RegisterForIniting(t, data.Item2);
                        }
                    }
                }
            }

            public GameObjectRegistrationData RegisterWhenInjecToGameObject<ToType, DependencyType>(GameObject gameObject, DependencyType instance)
            {
                _gameObjectToComponentMap[gameObject].Add(typeof(DependencyType), instance);

                return this;
            }

            public GameObjectRegistrationData RegisterWhenInjectoTo<ToType, DependencyType>(DependencyType instance)
            {
                if(!_typeToForInitingMap.ContainsKey(typeof(ToType)))
                {
                    throw new Exception($"=== {typeof(ToType).Name} doesn't exist first register that ===");
                }

                var dataList = _typeToForInitingMap[typeof(ToType)];

                foreach (var data in dataList)
                {
                    _gameObjectToComponentMap[data.Item1].Add(typeof(DependencyType), instance);
                }

                return this;
            }

            private void CheckForIniting<T>(T component) where T: Component
            {
                var t = typeof(T);
                var initInfo = t.GetMethod("Init");
                if(initInfo != null)
                {
                    //Debug.Log($"{t.Name} registering for Init");
                    if (!_typeToForInitingMap.ContainsKey(t))
                    {
                        _typeToForInitingMap.Add(t, new List<(GameObject, object)>());
                    }

                    _typeToForInitingMap[t].Add((component.gameObject, component));
                }
            }
        }

        public class GOComponentDependencyInjector
        {
            GameObject _gameObject;

            Dictionary<Type, List<Component>> _typeToComponentListMap;
            Dictionary<GameObject, Component> _gameObjectToComponentMap;
            Dictionary<GameObject, Dictionary<Type, Component>> _gameObjectToTypeToComponentMap;

            public GOComponentDependencyInjector(GameObject gameObject)
            {
                _gameObject = gameObject;

                _gameObjectToComponentMap = new Dictionary<GameObject, Component>();
                _typeToComponentListMap = new Dictionary<Type, List<Component>>();
                _gameObjectToTypeToComponentMap = new Dictionary<GameObject, Dictionary<Type, Component>>();
            }

            public void RegisterSingle<T>() where T:Component
            {
                var component = FindComponentAtRoot<T>();

                var t = typeof(T);

                var go = component.gameObject;

                if (!_gameObjectToComponentMap.ContainsKey(go))
                    _gameObjectToComponentMap.Add(go, component);

                if (!_typeToComponentListMap.ContainsKey(t))
                    _typeToComponentListMap.Add(t, new List<Component>());

                _typeToComponentListMap[t].Add(component);


                if (!_gameObjectToTypeToComponentMap.ContainsKey(go))
                {
                    _gameObjectToTypeToComponentMap.Add(go, new Dictionary<Type, Component>());
                    _gameObjectToTypeToComponentMap[go].Add(t, component);
                }
                else
                    _gameObjectToTypeToComponentMap[go].Add(t, component);
            }

            private T FindComponentAtRoot<T>() where T:Component
            {
                var t = typeof(T);

                var component = _gameObject.GetComponentInChildren<T>();

                if (component == null)
                    throw new Exception($"=== Root Name: {_gameObject.name} do not contains any {t.Name} named component on self or children ===");

                return component;
            }

            private T[] FindComponentCollectionAtRoot<T>() where T: Component
            {
                var t = typeof(T);

                var componentCollection = _gameObject.GetComponentsInChildren<T>();

                if (componentCollection == null)
                    throw new Exception($"=== Root Name: {_gameObject.name} do not contains any {t.Name} named component collection on self or children ===");

                return componentCollection;
            }
        }
    }
}
