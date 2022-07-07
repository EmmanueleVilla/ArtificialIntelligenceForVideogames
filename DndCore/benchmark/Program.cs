using Benchmark;
using Benchmark.Core.Map.Impl;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using DndCore.DI;
using DndCore.Utils.Log;
using System;
using System.Collections.Generic;

namespace benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var time = DateTime.Now;
            new ActionSequenceBuilderDemo();
            DndModule.Get<ILogger>().WriteLine(((DateTime.Now - time).TotalSeconds).ToString());
            Console.ReadLine();

            /*
            DndModule.Get<ILogger>().WriteLine("Starting..");
            var benchmark = new UCSBenchmark();
            benchmark.GlobalSetup();
            DndModule.Get<ILogger>().WriteLine("Map Initialized..");
            for (int i = 0; i < 5000; i++)
            {
                benchmark.FindPathGargantuanCreature();
            }
            DndModule.Get<ILogger>().WriteLine("Done.");
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
