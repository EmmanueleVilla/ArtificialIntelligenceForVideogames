using DndCore.Utils.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Utils.Log
{
    public class NoLogger : ILogger
    {
        public void WriteLine(string message)
        {
        }
    }
}
