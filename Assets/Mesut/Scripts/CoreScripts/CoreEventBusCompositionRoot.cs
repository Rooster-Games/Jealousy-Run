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
            AssemblyInstanceCreator assemblyInstanceCreator = initParameters.AssemblyInstanceCreator;
            // TODO: bir kere cekebilirim assembly bilgisini
            Type type = typeof(IEventBusCompositionRoot);

            var instance = (IEventBusCompositionRoot)assemblyInstanceCreator.FindAndCreateInstanceImpOfInterface(type);
            instance.Init(eventBus);

            eventBus.Raise<OnGameStarted>();
            eventBus.Raise<OnGameWin>();
            eventBus.Raise<OnGameFail>();

            // TODO:
            // Hangi class hangi evente baglaniyor
            // Bunun bilgisini gosteren bir
            // ekran yaz
        }

        public class InitParameters
        {
            public IEventBus EventBus { get; set; }
            public AssemblyInstanceCreator AssemblyInstanceCreator { get; set; }
        }
    }

    public interface IEventBusCompositionRoot
    {
        void Init(IEventBus eventBus);
    }
}