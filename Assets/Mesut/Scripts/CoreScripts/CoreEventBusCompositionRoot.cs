using System.Reflection;
using System.Linq;
using System;
using GameCores.CoreEvents;

namespace GameCores
{
    public class CoreEventBusCompositionRoot
    {
        public void Init(InitParameters initParameters)
        {
            IEventBus eventBus = initParameters.EventBus;
            // AssemblyInstanceCreator assemblyInstanceCreator = initParameters.AssemblyInstanceCreator;
            // TODO: bir kere cekebilirim assembly bilgisini
            // Type type = typeof(IEventBusCompositionRoot);

            //var instance = (IEventBusCompositionRoot)assemblyInstanceCreator.FindAndCreateInstanceImpOfInterface(type);
            //instance.Init(eventBus);

            var assembly = typeof(IEventData).Assembly;
            var eventTypes = assembly.GetTypes().Where(x => (x.IsClass || x.IsValueType) && typeof(IEventData).IsAssignableFrom(x));

            var t = typeof(IEventBus);

            var methodInfo = t.GetMethod("Raise");

            foreach (var eventType in eventTypes)
            {
                var genericMethod = methodInfo.MakeGenericMethod(new System.Type[] { eventType });
                genericMethod.Invoke(eventBus, null);
            }

        }

        public class InitParameters
        {
            public IEventBus EventBus { get; set; }
            // public AssemblyInstanceCreator AssemblyInstanceCreator { get; set; }
        }
    }

    public interface IEventBusCompositionRoot
    {
        void Init(IEventBus eventBus);
    }
}