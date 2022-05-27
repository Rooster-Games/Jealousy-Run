using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCores
{
    public class EventBus : IEventBus
    {
        public EventBus()
        {
            Debug.Log("EventBus Created");
            var assembly = typeof(IEventData).Assembly;
            var eventTypes = assembly.GetTypes().Where(x => (x.IsClass || x.IsValueType) && typeof(IEventData).IsAssignableFrom(x));

            var t = typeof(IEventBus);
            var methodInfo = t.GetMethod("Raise");

            foreach (var eventType in eventTypes)
            {
                var genericMethod = methodInfo.MakeGenericMethod(new Type[] { eventType });
                genericMethod.Invoke(this, null);
            }
        }

        Dictionary<Type, IActionData> _typeToActionMap = new Dictionary<Type, IActionData>();
        Dictionary<Type, IEventData> _typeToDataMap = new Dictionary<Type, IEventData>();

        public void Register<T>(Action<T> action) where T : IEventData
        {
            Type type = typeof(T);
            var registeredAction = _typeToActionMap[type];
            var actionData = (ActionData<T>)registeredAction;
            actionData.Register(action);
        }

        public void Fire<T>(T data = default) where T : IEventData
        {
            Type type = typeof(T);
            var registeredAction = _typeToActionMap[type];
            var actionData = (ActionData<T>)registeredAction;
            actionData.Fire(data);
        }

        public void Raise<T>() where T : IEventData, new()
        {
            Type type = typeof(T);
            if (_typeToActionMap.ContainsKey(type)) return;

            var actionData = new ActionData<T>();

            _typeToActionMap.Add(type, actionData);

            var data = new T();
            _typeToDataMap.Add(type, data);
        }

        public T GetData<T>() where T : IEventData, new()
        {
            Type type = typeof(T);
            return (T)_typeToDataMap[type];
        }

        public void UnRegister<T>(Action<T> action) where T : IEventData
        {
            Type type = typeof(T);

            var registeredActionData = _typeToActionMap[type];
            var actionData = (ActionData<T>)registeredActionData;
            actionData.UnRegister(action);
        }

        private interface IActionData
        {
        }

        private class ActionData<T> : IActionData where T : IEventData
        {
            protected Action<T> _action;

            public void Register(Action<T> action)
            {
                _action += action;
            }

            public void UnRegister(Action<T> action)
            {
                _action -= action;
            }

            public void Fire(T data)
            {
                _action?.Invoke(data);
            }
        }
    }

    public interface IEventData { }
}