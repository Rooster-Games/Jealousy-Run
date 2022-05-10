using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace GameCores
{
    public class EventBusInfoSO : ScriptableObject
    {
        [ReadOnly]
        [SerializeField] List<string> _allEventNames;
        [HorizontalLine(2, EColor.Green)]
        [ReadOnly]
        [SerializeField] List<EventInfo> _eventRegisterInfo;
        [ReadOnly]
        [SerializeField] List<EventInfo> _eventFireInfo;


        bool _isInitialized;

        [Button("Clear")]
        public void Init()
        {
            _allEventNames = new List<string>();
            _isInitialized = true;
            _eventRegisterInfo = new List<EventInfo>();
            _eventFireInfo = new List<EventInfo>();
        }

        public void AddEventInfo(string eventName)
        {
            if (!_allEventNames.Contains(eventName))
                _allEventNames.Add(eventName);
        }

        public void AddRegisterInfo(string eventName, string className, string methodName)
        {
            var classAndMethodName = $"Class: {className}, Method:{methodName}";
            foreach (var registeredInfo in _eventRegisterInfo)
            {
                if (registeredInfo.EventName.Equals(eventName))
                {
                    registeredInfo.AddClassAndMethodName(classAndMethodName);
                    return;
                }
            }

            var eventInfo = new EventInfo();
            eventInfo.AddEventName(eventName);

            eventInfo.AddClassAndMethodName(classAndMethodName);

            _eventRegisterInfo.Add(eventInfo);
        }

        public void AddFireInfo(string eventName, string className)
        {
            foreach (var registeredInfo in _eventFireInfo)
            {
                if (registeredInfo.EventName.Equals(eventName))
                    return;
            }

            var eventInfo = new EventInfo();
            eventInfo.AddEventName(eventName);

            var classAndMethodName = $"Class: {className}";
            eventInfo.AddClassAndMethodName(classAndMethodName);
            _eventFireInfo.Add(eventInfo);
        }

        // event name
           // class name - method name

        [System.Serializable]
        public class EventInfo
        {
            [SerializeField] string _eventName;
            [SerializeField] List<string> _classAndMethodName;

            public string EventName => _eventName;

            public EventInfo()
            {
                _classAndMethodName = new List<string>();
            }

            public void AddEventName(string eventName)
            {
                _eventName = eventName;
            }

            public void AddClassAndMethodName(string classAndMethodName)
            {
                foreach (var name in _classAndMethodName)
                {
                    if (name.Equals(classAndMethodName))
                        return;
                }

                _classAndMethodName.Add(classAndMethodName);
            }
        }
    }
}