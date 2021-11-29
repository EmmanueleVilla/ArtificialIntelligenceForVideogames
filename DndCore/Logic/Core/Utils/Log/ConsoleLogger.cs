﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utils.Log
{
    public class ConsoleLogger : ILogger
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
