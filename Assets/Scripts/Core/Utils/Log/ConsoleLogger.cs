using System;
using System.Collections.Generic;
using System.Text;

namespace dnd.Source.Utils.Log
{
    class ConsoleLogger : ILogger
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
