﻿using DndCore.Map;
using DndCore.Utils.Log;
using Logic.Core;
using Logic.Core.Battle;
using Logic.Core.Battle.ActionBuilders;
using Logic.Core.Dice;
using Logic.Core.GOAP.Actions;
using Logic.Core.Graph;
using Logic.Core.Map;
using System;
using System.Collections.Generic;
using ConsoleLogger = DndCore.Utils.Log.ConsoleLogger;
using ILogger = DndCore.Utils.Log.ILogger;

namespace DndCore.DI
{
    public static class DndModule
    {
        public class WriteToFileBool
        {
            public bool ShouldWrite { get; set; }
        }
        private static Dictionary<Type, Func<object>> factories = new Dictionary<Type, Func<object>>();
        private static Dictionary<Type, Func<object>> singletonsFactories = new Dictionary<Type, Func<object>>();
        private static Dictionary<Type, object> singletons = new Dictionary<Type, object>();
        public static void RegisterRulesForTest()
        {
            singletons.Clear();
            factories.Clear();
            factories.Add(typeof(ILogger), () => new MultiLogger(new List<ILogger> { new ConsoleLogger(), new FileLogger() }));
        }

        public static void RegisterRules(bool enableLogs = true, ILogger logger = null, int? seed = null, bool writeToFile = false)
        {
            singletons.Clear();
            factories.Clear();
            singletonsFactories.Clear();
            if (seed == null || !seed.HasValue)
            {
                singletonsFactories.Add(typeof(Random), () => new Random());
            } else
            {
                singletonsFactories.Add(typeof(Random), () => new Random(seed.Value));
            }
            singletonsFactories.Add(typeof(IDndBattle), () => new DndBattle());

            factories.Add(typeof(WriteToFileBool), () => new WriteToFileBool { ShouldWrite = writeToFile });
            factories.Add(typeof(IMapBuilder), () => new CsvFullMapBuilder());
            factories.Add(typeof(IDiceRoller), () => new DiceRoller());
            factories.Add(typeof(UniformCostSearch), () => new UniformCostSearch());
            factories.Add(typeof(ISpeedCalculator), () => new SpeedCalculator());
            factories.Add(typeof(IActionBuildersWrapper), () => new ActionBuildersWrapper());
            factories.Add(typeof(IActionSequenceBuilder), () => new ActionSequenceBuilder());
            factories.Add(typeof(IActionBuildersCleanup), () => new ActionBuildersCleanup());
            if(enableLogs)
            {
                if (logger != null)
                {
                    factories.Add(typeof(ILogger), () => logger);
                }
                else
                {
                    factories.Add(typeof(ILogger), () => new MultiLogger(new List<ILogger> { new ConsoleLogger(), new FileLogger() }));
                }
            } else
            {
                factories.Add(typeof(ILogger), () => new MultiLogger(new List<ILogger>()));
            }
        }

        public static T Get<T>() where T: class
        {
            if (singletonsFactories.ContainsKey(typeof(T)) && !singletons.ContainsKey(typeof(T)))
            {
                singletons.Add(typeof(T), singletonsFactories[typeof(T)]() as T);
            }

            if (singletons.ContainsKey(typeof(T)))
            {
                return singletons[typeof(T)] as T;
            }

            if(factories.ContainsKey(typeof(T)))
            {
                return factories[typeof(T)]() as T;
            }

            throw new Exception("Did not found factory for type " + typeof(T));
        }
    }
}
