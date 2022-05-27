using System;

namespace GameCores
{
    public interface IEventBus
    {
        void Fire<T>(T data = default) where T : IEventData;
        T GetData<T>() where T : IEventData, new();
        void Raise<T>() where T : IEventData, new();
        void Register<T>(Action<T> action) where T : IEventData;
        void UnRegister<T>(Action<T> action) where T : IEventData;
    }
}