using BenchmarkDotNet.Running;
using System;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<DictionaryBenchmark>();
            Console.WriteLine(summary);
            Console.ReadLine();
        }
    }
}
