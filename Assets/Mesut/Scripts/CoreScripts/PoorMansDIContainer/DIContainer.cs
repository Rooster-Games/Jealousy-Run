using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JR;
using UnityEngine;

namespace DIC
{
    public class DIContainer
    {
        private static DIContainer _instance;
        public static DIContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DIContainer();
                }

                return _instance;
            }
        }

        Dictionary<Type, Dictionary<Type, object>> _whenInjectoToTypeMap; // dependent to dependency type - object map
        List<object> _forInitingObjectList;
        Dictionary<object, Dictionary<Type, object>> _whenInjectToMap;
        Dictionary<Type, object> _registeredTypeToInstance;

        List<GameObjectInjector> _createdGOInjectors;

        List<IChecker> _checkerList;
        Dictionary<IChecker, (Type DependentType, Type DependencyType, object Dependency)> _checkerToDependentTypeToDependencyInstanceMap;
        SortedDictionary<int, List<object>> _sortedForInitingObjectListMap;

        List<IBindingData> _bindingDataList;

        bool _isInitingStarted;

        List<GameObjectInjector> _afterInitingCreatedGOInjectors;

        DIContainer()
        {
            _forInitingObjectList = new List<object>();
            _whenInjectToMap = new Dictionary<object, Dictionary<Type, object>>();
            _registeredTypeToInstance = new Dictionary<Type, object>();
            _createdGOInjectors = new List<GameObjectInjector>();
            _whenInjectoToTypeMap = new Dictionary<Type, Dictionary<Type, object>>();
            _checkerList = new List<IChecker>();
            _checkerToDependentTypeToDependencyInstanceMap = new Dictionary<IChecker, (Type DependentType, Type DependencyType, object Dependency)>();
            _sortedForInitingObjectListMap = new SortedDictionary<int, List<object>>();
            _bindingDataList = new List<IBindingData>();
            _afterInitingCreatedGOInjectors = new List<GameObjectInjector>();
        }

        public BindingData<T> Register<T>(int sortingOrder = 0)
        {
            var bindingData = new BindingData<T>(sortingOrder);

            _bindingDataList.Add(bindingData);
            return bindingData;
        }

        public void RegisterSingle<T>(T instance, bool checkForIniting = true, Type registerType = null, int sortingOrder = 0, [CallerMemberName] string callerName = "", [CallerFilePath] string callerFilePath = "")
        {
            // instance.CheckForIniting();
            if(checkForIniting)
                CheckForIniting(instance, sortingOrder);

            var t = typeof(T);
            if (registerType != null)
                t = registerType;

            if(_registeredTypeToInstance.ContainsKey(t))
            {
                Debug.Log($"=== {t.Name} Has Been Already Registered As Single. ===");
                Debug.Log($"=== Caller File Path: {callerFilePath}");
                Debug.Log($"=== Caller Name: {callerName}");
            }

            _registeredTypeToInstance.Add(t, instance);
        }

        public void RegisterWithCheck<CheckerType>(Type dependentType, Type dependencyType, object dependency, CheckerType checker) where CheckerType : IChecker
        {
            _checkerList.Add(checker);
            _checkerToDependentTypeToDependencyInstanceMap.Add(checker, (dependentType, dependencyType, dependency));
        }

        public void RegisterWhenInjectTo(Type dependentType, object dependency, Type dependencyType = null)
        {
            if (dependencyType == null)
                dependencyType = dependency.GetType();

            if (!_whenInjectoToTypeMap.ContainsKey(dependentType))
            {
                _whenInjectoToTypeMap.Add(dependentType, new Dictionary<Type, object>());
            }

            if (!_whenInjectoToTypeMap[dependentType].ContainsKey(dependencyType))
            {
                _whenInjectoToTypeMap[dependentType].Add(dependencyType, dependency);
            }
        }

        public void RegisterWhenInjectTo(object dependent, object dependency, Type dependencyType = null)
        {
            if (dependencyType == null)
                dependencyType = dependency.GetType();
            // var dependencyType = dependency.GetType();


            if (!_whenInjectToMap.ContainsKey(dependent))
                _whenInjectToMap.Add(dependent, new Dictionary<Type, object>());

            if (_whenInjectToMap[dependent].ContainsKey(dependencyType))
                Debug.Log($"=== Dependency has already get their when inject to type of {dependencyType.Name} ===");

            _whenInjectToMap[dependent].Add(dependencyType, dependency);
        }
        public void RegisterWhenInjectTo<T>(object dependency, Type dependencyType = null)
        {
            var dependentType = typeof(T);
            RegisterWhenInjectTo(dependentType, dependency, dependencyType);
        }

        public GameObjectInjector RegisterGameObject(GameObject gameObject)
        {
            var goInjector = new GameObjectInjector(gameObject);
                _createdGOInjectors.Add(goInjector);

            return goInjector;
        }

        private void CheckForIniting<T>(T instance, int sortingIndex)
        {
            var t = typeof(T);
            var methodInfo = t.GetMethod("Init");

            if (methodInfo != null)
                AddInstanceForIniting(instance, sortingIndex);

        }

        public void AddInstanceForIniting<T>(T instance, int sortingIndex = 0)
        {
            if (instance == null)
                throw new System.Exception($"=== Instance of {typeof(T).Name} Type is Null cannot Register for initing ===");

            _forInitingObjectList.Add(instance);

            if (!_sortedForInitingObjectListMap.ContainsKey(sortingIndex))
                _sortedForInitingObjectListMap.Add(sortingIndex, new List<object>());
            _sortedForInitingObjectListMap[sortingIndex].Add(instance);
        }

        private void Reset()
        {
            _instance = new DIContainer();
        }

        public void Resolve()
        {
            _isInitingStarted = true;
            foreach (var bindingData in _bindingDataList)
            {
                bindingData.Resolve();
            }

            //Debug.Log("Resolve");
            foreach (var goInjector in _createdGOInjectors)
            {
                goInjector.Resolve();
            }

            foreach (var checker in _checkerList)
            {
                var checkerType = checker.GetType();
                var methodInfo = checkerType.GetMethod("Init");

                var methodParameterType = methodInfo.GetParameters()[0].ParameterType;
                var fieldsInfo = methodParameterType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                var initParameterInstance = Activator.CreateInstance(methodParameterType);

                foreach (var fieldInfo in fieldsInfo)
                {
                    var fieldType = fieldInfo.FieldType;
                    object fieldInstance = null;
                    if(_whenInjectToMap.TryGetValue(checker, out var dependentTypeToInstanceMap))
                    {
                        if(dependentTypeToInstanceMap.TryGetValue(fieldType, out var whenIncjectToInstance))
                        {
                            fieldInstance = whenIncjectToInstance;
                        }
                        else if (_registeredTypeToInstance.TryGetValue(fieldType, out var registeredInstance))
                        {
                            fieldInstance = registeredInstance;
                        }
                    }

                    if(fieldInstance == null)
                    {
                        Debug.Log("For Checker: " + checkerType.Name);
                        Debug.Log("Field Name: " + fieldType.Name + " Couldn't Found.");
                    }

                    fieldInfo.SetValue(initParameterInstance, fieldInstance);
                }

                methodInfo.Invoke(checker, new object[] { initParameterInstance });

                if(checker.Check())
                {
                    var data = _checkerToDependentTypeToDependencyInstanceMap[checker];

                    RegisterWhenInjectTo(data.DependentType, data.Dependency, data.DependencyType);
                }
            }

            foreach (var index in _sortedForInitingObjectListMap.Keys)
            {
                var forInitingObjectList = _sortedForInitingObjectListMap[index];
                foreach (var instanceForIniting in forInitingObjectList)
                {
                    var dependentType = instanceForIniting.GetType();
                    var interfaceTypes = dependentType.GetInterfaces().ToList();
                    interfaceTypes.Add(dependentType);

                    //Debug.Log("Initing Type: " + t.Name);

                    var methodInfo = dependentType.GetMethod("Init");
                    var initParametersInfos = methodInfo.GetParameters();
                    var initParametersType = initParametersInfos[0].ParameterType;

                    var initParameterInstance = Activator.CreateInstance(initParametersType);

                    var fieldsInfo = initParametersType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (var fieldInfo in fieldsInfo)
                    {
                        var fieldType = fieldInfo.FieldType;

                        object fieldInstance = null;

                        if (_whenInjectToMap.TryGetValue(instanceForIniting, out var typeInstanceMap))
                            if (typeInstanceMap.TryGetValue(fieldType, out object whenInjectToInstance))
                                fieldInstance = whenInjectToInstance;

                        foreach (var interfaceType in interfaceTypes)
                        {
                            if (fieldInstance == null && _whenInjectoToTypeMap.TryGetValue(interfaceType, out var dependencyTypeToInstanceMap))
                                if (dependencyTypeToInstanceMap.TryGetValue(fieldType, out var dependencyInstance))
                                    fieldInstance = dependencyInstance;

                            if (fieldInstance != null)
                                break;
                        }

                        if (fieldInstance == null && _registeredTypeToInstance.TryGetValue(fieldType, out object registeredInstance))
                            fieldInstance = registeredInstance;

                        if (fieldInstance == null)
                        {
                            Debug.Log($"For {dependentType.Name}: ");
                            Debug.Log($"{fieldType.Name} is not found");
                        }

                        fieldInfo.SetValue(initParameterInstance, fieldInstance);
                    }

                    methodInfo.Invoke(instanceForIniting, new object[] { initParameterInstance });
                }
            }

            Reset();
        }
    }

    public interface IBindingData
    {
        void Resolve();
    }

    public class BindingData<T> : IBindingData
    {
        private Type _registerType; // interace Type
        private Type _actualType;  // class self Type

        int _sortingOrder;

        bool _registeredConcreteType;
        bool _hasDecorator;

        List<Type> _decoratorTypeList;

        public BindingData(int sortingOrder)
        {
            _registerType = typeof(T);
            _sortingOrder = sortingOrder;
        }

        public BindingData<T> AddDecorator<DecoratorType>() where DecoratorType : T
        {
            _hasDecorator = true;

            if (_decoratorTypeList == null) _decoratorTypeList = new List<Type>();

            _decoratorTypeList.Add(typeof(DecoratorType));

            return this;
        }

        public BindingData<T> RegisterConcreteType<RegisteredType>() where RegisteredType: T
        {
            _registeredConcreteType = true;

            _actualType = typeof(RegisteredType);

            return this;
        }

        public void Resolve()
        {
            if (_registeredConcreteType)
            {
                if (!_registerType.IsAssignableFrom(_actualType))
                {
                    throw new Exception($"=== {_actualType.Name} is not subclass or implementation of {_registerType} ===");
                }

                var instance = (T)Activator.CreateInstance(_actualType);

                if (_hasDecorator)
                {
                    //Debug.Log($"=== Registered Type: {_registerType.Name} ===");
                    //Debug.Log($"=== Concrete Type: {_actualType.Name} ===");
                    T beforeCreated = instance;

                    foreach (var decoratorType in _decoratorTypeList)
                    {
                        beforeCreated = (T)Activator.CreateInstance(decoratorType, new object[] { beforeCreated });
                        //Debug.Log($"=== Decorated With type of {decoratorType.Name} ===");
                    }

                    DIContainer.Instance.RegisterSingle<T>(beforeCreated, sortingOrder: _sortingOrder);

                    //Debug.Log("==============");
                }
                else
                {
                    DIContainer.Instance.RegisterSingle<T>(instance, sortingOrder: _sortingOrder);

                    //Debug.Log($"Registered Type: {_registerType}");
                    //Debug.Log($"Actual Type: {_actualType}");
                }
            }
            else
            {
                var instance = (T)Activator.CreateInstance(_registerType);
                DIContainer.Instance.RegisterSingle(instance, sortingOrder: _sortingOrder);
                //Debug.Log($"=== Registered Type: {_registerType.Name} ===");
            }
        }
    }
}