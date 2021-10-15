using dnd.Source.Utils.Log;
using Splat;
using System;
using System.Collections.Generic;
using ConsoleLogger = dnd.Source.Utils.Log.ConsoleLogger;
using ILogger = dnd.Source.Utils.Log.ILogger;

namespace dnd.Source.DI
{
    class DndModule
    {
        public void RegisterRules()
        {
            // Registering Random as singleton to mock it in tests
            var random = new Random();
            Locator.CurrentMutable.RegisterConstant(random);

            // Registering log handlers
            Locator.CurrentMutable.RegisterConstant(new MultiLogger(new List<ILogger> { new ConsoleLogger(), new FileLogger() }));
        }
    }
}
