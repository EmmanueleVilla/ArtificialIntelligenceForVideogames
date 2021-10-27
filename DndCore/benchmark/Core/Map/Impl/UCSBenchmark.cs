using BenchmarkDotNet.Attributes;
using Core.DI;
using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Bestiary;
using Logic.Core.Graph;
using Logic.Core.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Core.Map.Impl
{
    [RPlotExporter]
    public class UCSBenchmark
    {
        IMap map;
        CellInfo cell;

        [GlobalSetup]
        public void GlobalSetup()
        {
            DndModule.RegisterRules();
            map = new CsvFullMapBuilder().FromCsv(File.ReadAllText("Core/Map/river.txt"));
            var creatures = new List<ICreature>() {
            new RatmanWithBow(),
            new RatmanWithClaw(),
            new RatmanWithDagger(),
            new RatmanWithStaff(),
            new Minotaur(),
            new HumanMaleRanger(),
            new HumanFemaleMonk(),
            new DwarfMaleWarrior(),
            new ElfFemaleWizard()
            };

            var random = DndModule.Get<System.Random>();
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
                    cell = map.GetCellInfo(x, y);
                }
            }
        }

        [Benchmark]
        public int FindPath()
        {
            return new UniformCostSearch().Search(cell, map).Count;
        }
    }
}
