using BenchmarkDotNet.Attributes;
using DndCore.Map;
using Logic.Core.Map;
using Logic.Core.Map.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Map.Impl
{
    [RPlotExporter]
    public class DndMapBenchmark
    {
        [Benchmark]
        public int ReadDictionaryMapValues()
        {
            var map = new DictionaryDndMap(25, 25, new CellInfo('T', 0));

            var h = 0;
            for (int rep = 0; rep < 1000; rep++)
            {
                for (int x = 0; x < 25; x++)
                {
                    for (int y = 0; y < 25; y++)
                    {
                        var cell = map.GetCellInfo(x, y);
                        h = cell.Height;
                    }
                }
            }
            return h;
        }

        [Benchmark]
        public int ReadArrayMapValues()
        {
            var map = new ArrayDndMap(25, 25, new CellInfo('T', 0));

            var h = 0;
            for (int rep = 0; rep < 1000; rep++)
            {
                for (int x = 0; x < 25; x++)
                {
                    for (int y = 0; y < 25; y++)
                    {
                        var cell = map.GetCellInfo(x, y);
                        h = cell.Height;
                    }
                }
            }
            return h;
        }
    }

}
