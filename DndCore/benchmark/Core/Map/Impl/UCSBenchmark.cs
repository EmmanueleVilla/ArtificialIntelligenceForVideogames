using BenchmarkDotNet.Attributes;
using Core.DI;
using Core.Map;
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
            map = new ArrayDndMap(50, 50, new CellInfo('G', 0));
            var random = DndModule.Get<System.Random>();

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    map.SetCell(x, y, new CellInfo('G', (byte)random.Next(2), null, x, y));
                }
            }
            var creatures = new List<ICreature>() {
            new RatmanWithBow(),
            new RatmanWithClaw(),
            new RatmanWithDagger(),
            new RatmanWithStaff(),
            new Minotaur(),
            new HumanMaleRanger(),
            new HumanFemaleMonk(),
            new DwarfMaleWarrior(),
            new ElfFemaleWizard(),
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
                        case Sizes.Medium:
                            mediumCell = map.GetCellInfo(x, y);
                            break;
                        case Sizes.Large:
                            largeCell = map.GetCellInfo(x, y);
                            break;
                        case Sizes.Huge:
                            hugeCell = map.GetCellInfo(x, y);
                            break;
                        case Sizes.Gargantuan:
                            gargantuanCell = map.GetCellInfo(x, y);
                            break;
                    }
                }
            }
        }

        [Benchmark]
        public int FindPathMediumCreature()
        {
            var number = new UniformCostSearch().Search(mediumCell, map).Count;
            return number;
        }

        [Benchmark]
        public int FindPathLargeCreature()
        {
            var number = new UniformCostSearch().Search(largeCell, map).Count;
            return number;
        }

        [Benchmark]
        public int FindPathHugeCreature()
        {
            var number = new UniformCostSearch().Search(hugeCell, map).Count;
            return number;
        }

        [Benchmark]
        public int FindPathGargantuanCreature()
        {
            var number = new UniformCostSearch().Search(gargantuanCell, map).Count;
            return number;
        }
    }
}
