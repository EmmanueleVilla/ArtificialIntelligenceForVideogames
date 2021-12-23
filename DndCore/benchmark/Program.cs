using Benchmark;
using Benchmark.Core.Map.Impl;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;

namespace benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            new ActionSequenceBuilderDemo();
            //var summary = BenchmarkRunner.Run<UCSBenchmark>();
            Console.ReadLine();

            /*
            Console.WriteLine("Starting..");
            var benchmark = new UCSBenchmark();
            benchmark.GlobalSetup();
            Console.WriteLine("Map Initialized..");
            for (int i = 0; i < 5000; i++)
            {
                benchmark.FindPathGargantuanCreature();
            }
            Console.WriteLine("Done.");
            Console.ReadLine();
            */
            /*
            Dictionary<int, int> test = new Dictionary<int, int>();
            for(int i=0; i<64;i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    test.Add((i << 6) + j, 0);
                }
            }
            */
        }
    }
}
