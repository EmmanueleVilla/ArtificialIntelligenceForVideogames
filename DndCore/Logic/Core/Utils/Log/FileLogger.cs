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
            fileName = $"{now:yyyy_MM_dd_HH_mm_ss}.txt";
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
