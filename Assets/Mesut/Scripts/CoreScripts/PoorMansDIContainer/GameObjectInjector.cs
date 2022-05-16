using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using DI;
using JR;
using UnityEngine;

namespace DIC
{
    public class GameObjectInjector
    {
        GameObject _gameObject;
        ComponentFinder _componentFinder;

        Dictionary<Type, object> _typeToFactoryMap;
        Dictionary<Type, Component> _typeToComponentMap;
        Dictionary<Type, List<Component>> _typeToComponentCollectionMap;
        Dictionary<Type, List<Type>> _dependencyTypeToDependentTypeMap;
        Dictionary<Type, List<FieldInfo>> _factoryToDependencyFieldInfoMap;
        Dictionary<Type, MethodInfo> _factoryTypeToMethodInfo;

        Dictionary<Type, Dictionary<Type, object>> _whenInjecToDependencyTypeMap;

        Dictionary<Type, List<Type>> _outterDependencyMap; // could something dependent to a component
        Dictionary<Type, object> _factoryCreatedTypeToInstanceMap;

        Dictionary<IChecker, (Type DependentType, Type DependencyType)> _checkerToDependentToDependencyMap;

        public GameObjectInjector(GameObject gameObject)
        {
            _gameObject = gameObject;
            _componentFinder = new ComponentFinder(gameObject);
            _typeToFactoryMap = new Dictionary<Type, object>();
            _typeToComponentMap = new Dictionary<Type, Component>();
            _typeToComponentCollectionMap = new Dictionary<Type, List<Component>>();
            _dependencyTypeToDependentTypeMap = new Dictionary<Type, List<Type>>();
            _factoryToDependencyFieldInfoMap = new Dictionary<Type, List<FieldInfo>>();
            _factoryTypeToMethodInfo = new Dictionary<Type, MethodInfo>();
            _whenInjecToDependencyTypeMap = new Dictionary<Type, Dictionary<Type, object>>();
            _outterDependencyMap = new Dictionary<Type, List<Type>>();
            _factoryCreatedTypeToInstanceMap = new Dictionary<Type, object>();
            _checkerToDependentToDependencyMap = new Dictionary<IChecker, (Type DependentType, Type DependencyType)>();
        }

        public GameObjectInjector RegisterComponent<T>(bool isActive = false) where T: Component
        {
            var component = _componentFinder.FindComponent<T>(isActive);

            AddComponentToDictionary(component);

            CheckForIniting(component);

            return this;
        }

        public GameObjectInjector RegisterWithCheck<DependentType, DependencyType, CheckerType>(CheckerType checker) where CheckerType: IChecker
        {
            _checkerToDependentToDependencyMap.Add(checker, (typeof(DependentType), typeof(DependencyType)));

            var t = typeof(CheckerType);

            var methodInfo = t.GetMethod("Init");
            var methodParameters = methodInfo.GetParameters()[0];

            var parameterType = methodParameters.ParameterType;
            var fieldInfos = parameterType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var fieldInfo in fieldInfos)
            {
                var fieldType = fieldInfo.FieldType;
                object registerObj = null;
                if (_typeToComponentMap.TryGetValue(fieldType, out var component))
                {
                    registerObj = component;
                }
                else if (_factoryCreatedTypeToInstanceMap.TryGetValue(fieldType, out var factoryCreated))
                {
                    registerObj = factoryCreated;
                }

                if (registerObj != null)
                    DIContainer.Instance.RegisterWhenInjectTo(checker, registerObj);
            }

            return this;
        }

        public GameObjectInjector RegisterWhenInjectTo<DependentType, DependencyType>()
        {
            var dependentType = typeof(DependentType);
            var dependencyType = typeof(DependencyType);

            if(!_outterDependencyMap.ContainsKey(dependentType))
            {
                _outterDependencyMap.Add(dependentType, new List<Type>());
            }

            _outterDependencyMap[dependentType].Add(dependencyType);

            return this;
        }

        public GameObjectInjector RegisterWhenInjectTo<T>(object dependency, Type dependencyType = null)
        {
            if (dependencyType == null)
                dependencyType = dependency.GetType();

            var dependentType = typeof(T);

            if (!_whenInjecToDependencyTypeMap.ContainsKey(dependentType))
                _whenInjecToDependencyTypeMap.Add(dependentType, new Dictionary<Type, object>());

            _whenInjecToDependencyTypeMap[dependentType].Add(dependencyType, dependency);

            return this;
        }

        public GameObjectInjector RegisterComponentCollection<T>(bool isActive = false) where T: Component
        {
            var componentCollection = _componentFinder.FindComponentCollection<T>(isActive);

            foreach (var component in componentCollection)
            {
                AddComponentToDictionary(component);
                CheckForIniting(component);
                Debug.Log("Component Type: " + component.GetType().Name);
            }

            return this;
        }

        public GameObjectInjector RegisterFactoryFor<T, FactoryType>()
        {
            var t = typeof(T);
            var factoryType = typeof(FactoryType);

            var createMethodInfo = factoryType.GetMethod("Create");

            if (createMethodInfo == null)
                throw new Exception($"=== {t.Name} Typed Factory Has No Create Method ===");

            _factoryTypeToMethodInfo.Add(typeof(FactoryType), createMethodInfo);

            var createMethodParameters = createMethodInfo.GetParameters();
            var createParametersType = createMethodParameters[0].ParameterType;

            if (!_factoryToDependencyFieldInfoMap.ContainsKey(factoryType))
            {
                _factoryToDependencyFieldInfoMap.Add(factoryType, new List<FieldInfo>());

                var fieldsInfo = createParametersType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                foreach (var fieldInfo in fieldsInfo)
                {
                    _factoryToDependencyFieldInfoMap[factoryType].Add(fieldInfo);
                }
            }

            if(!_typeToFactoryMap.ContainsKey(t))
            {
                var factory = Activator.CreateInstance(typeof(FactoryType));
                _typeToFactoryMap.Add(t, factory);
            }

            return this;
        }

        private void AddComponentToDictionary<T>(T component) where T : Component
        {
            var t = typeof(T);
            if (_typeToComponentMap.ContainsKey(t))
            {
                _typeToComponentMap.Remove(t);

                if (!_typeToComponentCollectionMap.ContainsKey(t))
                    _typeToComponentCollectionMap.Add(t, new List<Component>());

                _typeToComponentCollectionMap[t].Add(component);
            }
            else
                _typeToComponentMap.Add(t, component);
        }

        private void CheckForIniting<T>(T instance) where T: Component
        {
            var t = typeof(T);
            var initMethodInfo = t.GetMethod("Init");

            if (initMethodInfo == null) return;

            var initMethodParameters = initMethodInfo.GetParameters();
            var initParametersType = initMethodParameters[0].ParameterType;

            RegisterDependenciesDependent(_dependencyTypeToDependentTypeMap, initParametersType, t);

            DIContainer.Instance.AddInstanceForIniting(instance, -1);
        }

        private void RegisterDependenciesDependent(Dictionary<Type, List<Type>> dependencyTToDependentTMap, Type parameterType, Type dependentType)
        {
            var fieldsInfo = parameterType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var fieldInfo in fieldsInfo)
            {
                var fieldType = fieldInfo.FieldType;

                if (!dependencyTToDependentTMap.ContainsKey(fieldType))
                    dependencyTToDependentTMap.Add(fieldType, new List<Type>());

                dependencyTToDependentTMap[fieldType].Add(dependentType);
            }
        }

        public void Resolve()
        {
            foreach (var dependencyType in _dependencyTypeToDependentTypeMap.Keys)
            {
                var dependentTypeList = _dependencyTypeToDependentTypeMap[dependencyType];

                foreach (var dependentType in dependentTypeList)
                {
                    if(_typeToComponentMap.TryGetValue(dependentType, out var dependentComponent))
                    {
                        if (_typeToFactoryMap.TryGetValue(dependencyType, out var factoryObj))
                        {
                            ResolveFactory(dependencyType, dependentComponent, factoryObj);
                        }
                        else if (_typeToComponentMap.TryGetValue(dependencyType, out var dependencyComponent))
                        {
                            DIContainer.Instance.RegisterWhenInjectTo(dependentComponent, dependencyComponent);
                        }
                        else if (_typeToComponentCollectionMap.TryGetValue(dependencyType, out var dependencyCollectionComponent))
                        {
                            DIContainer.Instance.RegisterWhenInjectTo(dependencyComponent, dependencyCollectionComponent);
                        }
                        else if (_whenInjecToDependencyTypeMap.TryGetValue(dependentType, out var dependencyTypeToInstanceMap))
                            if (dependencyTypeToInstanceMap.TryGetValue(dependencyType, out var dependency))
                                DIContainer.Instance.RegisterWhenInjectTo(dependentComponent, dependency);
                    }
                }
            }

            foreach (var dependentType in _outterDependencyMap.Keys)
            {
                var dependencyList = _outterDependencyMap[dependentType];

                foreach (var dependencyType in dependencyList)
                {
                    if (_typeToComponentMap.TryGetValue(dependencyType, out var dependency))
                    {
                        DIContainer.Instance.RegisterWhenInjectTo(dependentType, dependency);
                    }
                    else if (_factoryCreatedTypeToInstanceMap.TryGetValue(dependencyType, out var factoryCreatedDependency))
                    {
                        DIContainer.Instance.RegisterWhenInjectTo(dependentType, factoryCreatedDependency);
                    }
                }
            }

            ResolveCheckers();
        }

        private void ResolveCheckers()
        {
            foreach (var checker in _checkerToDependentToDependencyMap.Keys)
            {
                var dependentToDependencyData = _checkerToDependentToDependencyMap[checker];

                var dependentType = dependentToDependencyData.DependentType;
                var dependencyType = dependentToDependencyData.DependencyType;

                object dependency = null;

                if (_typeToComponentMap.TryGetValue(dependencyType, out var dependencyComponent))
                    dependency = dependencyComponent;
                else if (_factoryCreatedTypeToInstanceMap.TryGetValue(dependencyType, out var factoryCreated))
                    dependency = factoryCreated;

                DIContainer.Instance.RegisterWithCheck(dependentType, dependencyType, dependency, checker);

            }
        }

        private void ResolveFactory(Type dependencyType, Component dependentComponent, object factoryObj)
        {
            var factoryType = factoryObj.GetType();
            object dependencyObj = null;

            if (_factoryCreatedTypeToInstanceMap.TryGetValue(dependencyType, out var createdObj))
                dependencyObj = createdObj;

            else if (_factoryTypeToMethodInfo.TryGetValue(factoryType, out var methodInfo))
            {
                var methodParameterType = methodInfo.GetParameters()[0].ParameterType;
                var methodParameterObj = Activator.CreateInstance(methodParameterType);

                if (_factoryToDependencyFieldInfoMap.TryGetValue(factoryType, out var dependencyFInfoList))
                {
                    foreach (var dependencyFInfo in dependencyFInfoList)
                    {
                        var fieldType = dependencyFInfo.FieldType;

                        if (_typeToComponentMap.TryGetValue(fieldType, out var component))
                        {
                            dependencyFInfo.SetValue(methodParameterObj, component);
                        }
                    }
                }
                else
                    throw new Exception("=== Factory error ===");

                dependencyObj = methodInfo.Invoke(factoryObj, new object[] { methodParameterObj });
                _factoryCreatedTypeToInstanceMap.Add(dependencyType, dependencyObj);
            }
            else
                throw new Exception("=== Factory method info ===");

            DIContainer.Instance.RegisterWhenInjectTo(dependentComponent, dependencyObj, dependencyType);
        }
    }

    public class ComponentFinder
    {
        GameObject _gameObject;

        public ComponentFinder(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public T FindComponent<T>(bool isActive) where T :Component
        {
            var component = GetComponentSelf<T>();

            if (component == null)
                component = GetComponentInChildren<T>(isActive);

            if (component == null)
                throw new System.Exception($"=== Couldn't find {typeof(T).Name}'s Component at {_gameObject.name}'s hierarchy! ===");

            component.CheckForIniting();
            return component;
        }

        public T[] FindComponentCollection<T>(bool isActive) where T: Component
        {
            var componentCollection = GetComponentsInChildren<T>(isActive);

            if (componentCollection == null)
                throw new Exception($"=== Couldn't find {nameof(T)}'s Component Collection at {_gameObject.name}'s hierarchy! ===");

            foreach (var component in componentCollection)
            {
                component.CheckForIniting();
            }

            return componentCollection;
        }

        private T GetComponentSelf<T>() where T : Component
        {
            var component = _gameObject.GetComponent<T>();

            return component;
        }

        private T GetComponentInChildren<T>(bool isActive) where T: Component
        {
            var component = _gameObject.GetComponentInChildren<T>(isActive);

            return component;
        }

        private T[] GetComponentsInChildren<T>(bool isActive) where T: Component
        {
            var components = _gameObject.GetComponentsInChildren<T>(isActive);

            return components;
        }
    }
}