using DndCore.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DndCore.Utils.Log
{
    public class ConsoleLogger : ILogger
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
