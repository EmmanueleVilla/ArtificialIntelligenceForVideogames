using System.Collections.Generic;

namespace Core.Utils.Log
{
    class MultiLogger : ILogger
    {
        private readonly List<ILogger> loggers;
        public MultiLogger(List<ILogger> loggersList)
        {
            this.loggers = loggersList;
        }
        public void WriteLine(string message)
        {
            loggers.ForEach(x => x.WriteLine(message));
        }
    }
}
