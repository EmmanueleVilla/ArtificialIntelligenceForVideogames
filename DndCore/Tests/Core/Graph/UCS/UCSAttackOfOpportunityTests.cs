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
                1,
                Loyalties.Ally,
                movements: new List<Speed>()
                {
                    new Speed(SpeedTypes.Walking, 99)
                }
                );
            var enemy = new MockedCreature(
                1,
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

                //0
                new MemoryEdge(
                    new List<CellInfo>() {
                        map.GetCellInfo(0,3)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) }
                    }, map.GetCellInfo(0,2), 1, 0 , true),

                //1
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,3) }
                    }, map.GetCellInfo(1,3), 1, 0 , true),

                //2
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,2) }
                    }, map.GetCellInfo(1,2), 1, 0 , true),

                //3
                new MemoryEdge(
                    new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,1) }
                    }, map.GetCellInfo(0,1), 2, 0 , true),

                //4
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,1) }
                    }, map.GetCellInfo(1,1), 2, 0 , true),

                //5
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,3)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,3) }
                    }, map.GetCellInfo(2,3), 2, 0 , true),

                //6
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,1) }
                    }, map.GetCellInfo(2,1), 2, 0 , true),

                //7
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,0) }
                    }, map.GetCellInfo(0,0), 3, 0 , true),

                //8
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) }
                    }, map.GetCellInfo(1,0), 3, 0 , true),


                //9
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(1,1)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Attacks, Attack = enemy.Attacks.First() }
                    }, map.GetCellInfo(2,0), 3, 10 , true),

                //10
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,3),
                        map.GetCellInfo(2,3)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,3) }
                    }, map.GetCellInfo(3,3), 3, 0 , true),

                //11
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,3),
                        map.GetCellInfo(2,3)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,2) },
                    }, map.GetCellInfo(3,2), 3, 0 , true),

                //12
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,1) },
                    }, map.GetCellInfo(3,1), 3, 0 , true),


                //13
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Attacks, Attack = enemy.Attacks.First() }
                    }, map.GetCellInfo(3,0), 3, 10 , true),

                //14
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) }
                    }, map.GetCellInfo(2,0), 4, 0 , true),

                //15
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,3),
                        map.GetCellInfo(2,3),
                        map.GetCellInfo(3,3)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Attacks, Attack = enemy.Attacks.First() }
                    }, map.GetCellInfo(4,3), 4, 10 , true),

                //16
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,3),
                        map.GetCellInfo(2,3),
                        map.GetCellInfo(3,3)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Attacks, Attack = enemy.Attacks.First() }
                    }, map.GetCellInfo(4,2), 4, 10 , true),

                //17
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,3),
                        map.GetCellInfo(2,3),
                        map.GetCellInfo(3,2)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,3) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Attacks, Attack = enemy.Attacks.First() }
                    }, map.GetCellInfo(4,1), 4, 10 , true),

                //18
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(1,2),
                        map.GetCellInfo(2,1),
                        map.GetCellInfo(3,1)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Attacks, Attack = enemy.Attacks.First() }
                    }, map.GetCellInfo(4,0), 4, 10 , true),

                //19
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,0) }
                    }, map.GetCellInfo(3,0), 5, 0 , true),

                //20
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0),
                        map.GetCellInfo(3,0)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,0) }
                    }, map.GetCellInfo(4,0), 6, 0 , true),

                //21
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0),
                        map.GetCellInfo(3,0)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,1) }
                    }, map.GetCellInfo(4,1), 6, 0 , true),

                //22
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0),
                        map.GetCellInfo(3,0),
                        map.GetCellInfo(4,1)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,2) }
                    }, map.GetCellInfo(4,2), 7, 0 , true),

                //23
                new MemoryEdge(new List<CellInfo>() {
                        map.GetCellInfo(0,3),
                        map.GetCellInfo(0,2),
                        map.GetCellInfo(0,1),
                        map.GetCellInfo(1,0),
                        map.GetCellInfo(2,0),
                        map.GetCellInfo(3,0),
                        map.GetCellInfo(4,1),
                        map.GetCellInfo(4,2)
                    }, new List<MovementEvent>() {
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,0) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,1) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,2) },
                        new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(4,3) }
                    }, map.GetCellInfo(4,3), 8, 0 , true),
            };
            var result = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(map.GetCellInfo(0, 3), map);
            Assert.AreEqual(expected, result);
        }
    }
}
