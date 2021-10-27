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
            var summary = BenchmarkRunner.Run<UCSBenchmark>();
            Console.ReadLine();
        }
    }
}
