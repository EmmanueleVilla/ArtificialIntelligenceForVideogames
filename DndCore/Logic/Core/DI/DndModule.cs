using Core.Map;
using Core.Utils.Log;
using Logic.Core;
using Logic.Core.Battle;
using Logic.Core.Dice;
using Logic.Core.Map;
using System;
using System.Collections.Generic;
using ConsoleLogger = Core.Utils.Log.ConsoleLogger;
using ILogger = Core.Utils.Log.ILogger;

namespace Core.DI
{
    public static class DndModule
    {
        private static Dictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>();
        private static Dictionary<Type, object> singletons = new Dictionary<Type, object>();

        public static void RegisterRulesForTest()
        {
            singletons.Clear();
            factories.Clear();
            factories.Add(typeof(ILogger), () => new MultiLogger(new List<ILogger> { new ConsoleLogger(), new FileLogger() }));
        }

        public static void RegisterRules(bool enableLogs = true)
        {
            singletons.Clear();
            factories.Clear();
            singletons.Add(typeof(Random), new Random());
            factories.Add(typeof(IMapBuilder), () => new CsvFullMapBuilder());
            factories.Add(typeof(IDiceRoller), () => new DiceRoller());
            factories.Add(typeof(IDndBattle), () => new DndBattle());
            if(enableLogs)
            {
                factories.Add(typeof(ILogger), () => new MultiLogger(new List<ILogger> { new ConsoleLogger(), new FileLogger() }));
            } else
            {
                factories.Add(typeof(ILogger), () => new MultiLogger(new List<ILogger>()));
            }
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
