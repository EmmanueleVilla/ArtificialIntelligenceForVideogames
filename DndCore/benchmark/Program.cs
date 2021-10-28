using Benchmark.Core.Map.Impl;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;

namespace benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var summary = BenchmarkRunner.Run<UCSBenchmark>(
                new DebugInProcessConfig()
                );
            Console.ReadLine();
            */

            Console.WriteLine("Starting..");
            var benchmark = new UCSBenchmark();
            benchmark.GlobalSetup();
            Console.WriteLine("Map Initialized..");
            for (int i = 0; i < 5000; i++)
            {
                benchmark.FindPathGargantuanCreature();
            }
            Console.WriteLine("Done.");
        }
    }
}
