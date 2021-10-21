using dnd.Source.Map;
using dnd.Source.Utils.Log;
using System;
using System.Collections.Generic;
using ConsoleLogger = dnd.Source.Utils.Log.ConsoleLogger;
using ILogger = dnd.Source.Utils.Log.ILogger;

namespace dnd.Source.DI
{
    public static class DndModule
    {
        private static Dictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>();
        private static Dictionary<Type, object> singletons = new Dictionary<Type, object>();

        public static void RegisterRules()
        {
            singletons.Add(typeof(Random), new Random());
            factories.Add(typeof(ILogger), () => new MultiLogger(new List<ILogger> { new ConsoleLogger(), new FileLogger() }));
            factories.Add(typeof(IMapBuilder), () => new CsvMapBuilder());
        }

        public static T Get<T>() where T: class
        {
            if(singletons.ContainsKey(typeof(T)))
            {
                return singletons[typeof(T)] as T;
            }

            if(factories.ContainsKey(typeof(T)))
            {
                return factories[typeof(T)]() as T;
            }

            return default(T);
        }
    }
}
