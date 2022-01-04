using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Map.Impl
{
    [RPlotExporter]
    public class TryCatchBenchmark
    {
        [Benchmark]
        public int ParseWithoutTryCatch()
        {
            int result = 0;
            for (int i = 0; i < 100; i++)
            {
                var input = "12.34";
                if (!int.TryParse(input, out result))
                {
                }
            }
            return result;
        }

        [Benchmark]
        public int ParseWithTryCatch()
        {
            int result = 0;
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    var input = "12.34";
                    if (!int.TryParse(input, out result))
                    {
                        throw new Exception("Error");
                    }
                }
                catch (Exception e)
                {
                }
            }
            return result;
        }
    }
}
