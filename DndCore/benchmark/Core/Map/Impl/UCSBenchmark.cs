using BenchmarkDotNet.Attributes;
using DndCore.DI;
using DndCore.Map;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Bestiary;
using Logic.Core.Graph;
using Logic.Core.Map.Impl;
using System.Collections.Generic;

namespace Benchmark.Core.Map.Impl
{
    [RPlotExporter]
    public class UCSBenchmark
    {
        ArrayDndMap map;
        CellInfo mediumCell;
        CellInfo largeCell;
        CellInfo hugeCell;
        CellInfo gargantuanCell;

        [GlobalSetup]
        public void GlobalSetup()
        {
            DndModule.RegisterRules(false);
            map = new ArrayDndMap(63, 63, new CellInfo('G', 0));
            var random = DndModule.Get<System.Random>();

            for (int x = 0; x < 63; x++)
            {
                for (int y = 0; y < 63; y++)
                {
                    map.SetCell(x, y, new CellInfo('G', (byte)random.Next(2), null, x, y));
                }
            }
            var creatures = new List<ICreature>() {
            new ElfFemaleWizard(),
            new ElfFemaleWizard(),
            new ElfFemaleWizard(),
            new ElfFemaleWizard(),
            new LargeMinotaur(),
            new LargeMinotaur(),
            new LargeMinotaur(),
            new HugeMinotaur(),
            new HugeMinotaur(),
            new GargantuanMinotaur()
            };

            foreach (var creature in creatures)
            {
                bool fit = false;
                while (!fit)
                {
                    var x = 0;
                    var y = 0;
                    if (creature.Loyalty == Loyalties.Ally)
                    {
                        x = random.Next(0, map.Width / 3);
                        y = random.Next(3, map.Height - 3);
                    }
                    else
                    {
                        x = random.Next(map.Width / 3 * 2, map.Width);
                        y = random.Next(3, map.Height - 3);
                    }
                    if (map.GetCellInfo(x, y).Terrain != 'G')
                    {
                        continue;
                    }
                    fit = map.AddCreature(creature, x, y);
                    switch(creature.Size)
                    {
                        case 1:
                            mediumCell = map.GetCellInfo(x, y);
                            break;
                        case 2:
                            largeCell = map.GetCellInfo(x, y);
                            break;
                        case 3:
                            hugeCell = map.GetCellInfo(x, y);
                            break;
                        case 4:
                            gargantuanCell = map.GetCellInfo(x, y);
                            break;
                    }
                }
            }
        }

        [Benchmark]
        public int FindPathMediumCreature()
        {
            var number = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(mediumCell, map).Count;
            return number;
        }

        [Benchmark]
        public int FindPathLargeCreature()
        {
            var number = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(largeCell, map).Count;
            return number;
        }

        [Benchmark]
        public int FindPathHugeCreature()
        {
            var number = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(hugeCell, map).Count;
            return number;
        }

        [Benchmark]
        public int FindPathGargantuanCreature()
        {
            var number = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(gargantuanCell, map).Count;
            return number;
        }
    }
}
