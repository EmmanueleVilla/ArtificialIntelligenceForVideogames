using Core.DI;
using Core.Map;
using Core.Utils.Log;
using Logic.Core;
using Logic.Core.Battle;
using Logic.Core.Battle.ActionBuilders;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Bestiary;
using Logic.Core.Dice;
using Logic.Core.GOAP.Actions;
using Logic.Core.Graph;
using Logic.Core.Map;
using Logic.Core.Map.Impl;
using Logic.Core.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.DndBattles.Mock;

namespace Benchmark
{
    public class MapProvider
    {
        public List<String> Maps = new List<string>()
        {
                "G9,G9,G6,G6,G6,G7,G6,G6,G7,G9,R0,R0,G7,G7,G7,G9,G9,G9,G9,G9\n" +
                "G9,G9,G6,G6,G6,G6,G6,G6,G7,G9,R0,R0,G4,G7,G7,G6,G6,G6,G9,G9\n" +
                "G9,G9,G6,G6,G6,G4,G4,G6,G7,G9,R0,R0,G4,G4,G4,G6,G6,G6,G9,G9\n" +
                "G9,G9,G6,G6,G6,G4,G4,G5,G5,G3,R0,R0,G4,G4,G4,G4,G4,G6,G9,G9\n" +
                "G9,G9,G4,G4,G4,G4,G4,G5,G3,G3,R0,R0,G4,G4,G4,G3,G3,G5,G9,G9\n" +
                "G9,G9,G4,G4,G4,G3,G3,G3,G3,S4,S4,S4,S4,G4,G4,G3,G3,G3,G3,G3\n" +
                "G9,G5,G4,G4,G4,G3,G3,G3,G3,R0,R0,R0,G4,G4,G4,G3,G3,G3,G2,G2\n" +
                "G3,G3,G3,G3,G3,G3,G3,G3,R0,R0,G1,R0,G2,G2,G2,G3,G3,G2,G2,G2\n" +
                "G3,G3,G3,G3,G3,G3,G3,G3,R0,G1,G1,R0,G2,G2,G2,S2,G3,G2,G2,G2\n" +
                "G3,G3,G3,G3,G3,G3,G3,G3,R0,G1,G1,R0,R0,G2,G2,S2,R0,R0,G2,G2\n" +
                "G1,G3,G3,G3,G3,G3,G3,R0,R0,G1,G1,R0,R0,R0,R0,S2,R0,R0,R0,R0\n" +
                "G1,G3,G3,G3,G3,G3,G3,R0,G1,G1,G1,G1,R0,R0,R0,S2,G1,G1,G1,G1\n" +
                "G1,G2,G2,R0,R0,R0,R0,R0,G1,G1,G1,G1,G1,G1,G1,S2,G1,G1,G1,G1\n" +
                "G1,G2,G2,R0,R0,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1\n" +
                "G1,G1,G1,R0,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1,G1"
        };

        public IMap BuildMap()
        {
            var mapIndex = DndModule.Get<Random>().Next(0, Maps.Count);
            var map = DndModule.Get<IMapBuilder>().FromCsv(Maps[mapIndex]);
            return map;
        }
    }

    public class ActionSequenceBuilderDemo
    {
        private List<char> validTerrains = new List<char>() { 'G' };

        public ActionSequenceBuilderDemo()
        {
            DndModule.RegisterRules(false, null, 0);
            var battle = DndModule.Get<IDndBattle>();

            var creatures = new EncounterProvider().BuildEncounter();

            var map = new MapProvider().BuildMap();
            var random = DndModule.Get<System.Random>();
            foreach (var creature in creatures)
            {
                bool fit = false;
                while (!fit)
                {
                    var startingX = creature.Loyalty == Loyalties.Ally ? 0 : map.Width / 3 * 2;
                    var endingX = creature.Loyalty == Loyalties.Ally ? map.Width / 3 : map.Width;
                    var x = random.Next(startingX, endingX);
                    var y = random.Next(3, map.Height - 3);
                    if (!validTerrains.Contains(map.GetCellInfo(x, y).Terrain))
                    {
                        continue;
                    }

                    fit = map.AddCreature(creature, x, y);
                }
            }

            battle.Init(map);
            var round = 1;
            ICreature first = null;
            while (true)
            {
                if(first == battle.GetCreatureInTurn())
                {
                    round++;
                    Console.WriteLine("Start round " + round);
                }
                if(first == null)
                {
                    first = battle.GetCreatureInTurn();
                }
                Console.WriteLine("Start turn of " + battle.GetCreatureInTurn().GetType().Name);
                battle.PlayTurn();
                battle.NextTurn();
                if (battle.Map.Creatures.All(c => c.Value.Loyalty == first.Loyalty))
                {
                    Console.WriteLine("Game end! " + first.Loyalty + " won");
                }
            }
        }
    }
}
