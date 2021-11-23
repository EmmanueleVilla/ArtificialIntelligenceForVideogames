using Core.Map;
using Logic.Core.Actions;
using Logic.Core.Battle;
using Logic.Core.Creatures;
using Logic.Core.Graph;
using Logic.Core.Map;
using Logic.Core.Movements;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks.Creatures;

namespace Tests.Core.Graph.UCS
{
    [TestFixture]
    class UCSAttackOfOpportunityTests
    {
        [Test]
        public void PreferLongerPathToAttackOfOpportunity()
        {
            var mapCsv = "" +
                "G,G,G,G,G\n" +
                "G,G,G,G,G\n" +
                "G,G,G,G,G\n" +
                "G,G,G,G,G";
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            var ally = new MockedCreature(
                Sizes.Medium,
                Loyalties.Ally,
                movements: new List<Speed>()
                {
                    new Speed(SpeedTypes.Walking, 99)
                }
                );
            var enemy = new MockedCreature(
                Sizes.Medium,
                Loyalties.Enemy,
                attacks: new List<Attack>()
                {
                    new Attack("stub", AttackTypes.WeaponMelee,
                        new List<Damage>() { new Damage(DamageTypes.Acid, 10, 0,0,0) })
                }
                );

            map.AddCreature(ally, 0, 3);
            map.AddCreature(enemy, 2, 2);
            var expected = new List<MemoryEdge>() {
                new MemoryEdge(
                    new List<CellInfo>() {
                        map.GetCellInfo(0,3)
                    }, new List<MovementEvent>(), map.GetCellInfo(0,2), 1, 0 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3)
                    }, new List<MovementEvent>(), map.GetCellInfo(1,2), 1, 0 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3)
                    }, new List<MovementEvent>(), map.GetCellInfo(1,3), 1, 0 , true),

                new MemoryEdge(
                    new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2)
                    }, new List<MovementEvent>(), map.GetCellInfo(0,1), 2, 0 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2)
                    }, new List<MovementEvent>(), map.GetCellInfo(1,1), 2, 0 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2)
                    }, new List<MovementEvent>(), map.GetCellInfo(2,1), 2, 0 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2)
                    }, new List<MovementEvent>(), map.GetCellInfo(2,3), 2, 0 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(0,0), 3, 0 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(1,0), 3, 0 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(1,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(2,0), 3, 10 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(3,0), 3, 10 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(3,1), 3, 0 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(3,2), 3, 0 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,3)
                    }, new List<MovementEvent>(), map.GetCellInfo(3,3), 3, 0 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0)
                    }, new List<MovementEvent>(), map.GetCellInfo(2,0), 4, 0 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1),
                        map.GetCellInfo(3,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(4,0), 4, 10 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1),
                        map.GetCellInfo(3,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(4,1), 4, 10 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1),
                        map.GetCellInfo(3,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(4,2), 4, 10 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1),
                        map.GetCellInfo(3,2)
                    }, new List<MovementEvent>(), map.GetCellInfo(4,3), 4, 10 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0)
                    }, new List<MovementEvent>(), map.GetCellInfo(3,0), 5, 0 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0),
                        map.GetCellInfo(3,0)
                    }, new List<MovementEvent>(), map.GetCellInfo(4,0), 6, 0 , true),
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0),
                        map.GetCellInfo(3,0)
                    }, new List<MovementEvent>(), map.GetCellInfo(4,1), 6, 0 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0),
                        map.GetCellInfo(3,0),
                        map.GetCellInfo(4,1)
                    }, new List<MovementEvent>(), map.GetCellInfo(4,2), 7, 0 , true),

                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0),
                        map.GetCellInfo(3,0),
                        map.GetCellInfo(4,1),
                        map.GetCellInfo(4,2)
                    }, new List<MovementEvent>(), map.GetCellInfo(4,3), 8, 0 , true),
            };
            var result = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(map.GetCellInfo(0, 3), map);
            Assert.AreEqual(expected, result);
        }
    }
}
