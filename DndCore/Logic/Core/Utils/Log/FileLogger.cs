using System;
using System.IO;

namespace Core.Utils.Log
{
    class FileLogger : ILogger
    {
        readonly string fileName;
        public FileLogger()
        {
            var now = DateTime.Now;
            fileName = $"{now:yyyy-MM-dd-HH:mm:ss}.txt";
        }

        public void WriteLine(string message)
        {
            using (StreamWriter outputFile = new StreamWriter(fileName, true))
            {
                outputFile.WriteLine(message);
            }
        }
    }
}
