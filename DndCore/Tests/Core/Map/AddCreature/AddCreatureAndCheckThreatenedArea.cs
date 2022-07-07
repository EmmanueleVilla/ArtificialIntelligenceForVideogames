using DndCore.Map;
using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Map.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks.Creatures;

namespace Tests.Core.Map.AddCreature
{
    [TestFixture]
    class AddCreatureAndCheckThreatenedArea: BaseDndTest
    {
        [Test]
        public void AddMediumCreatureWithBaseAttack()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(1, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>())
                    });
            map.AddCreature(creature, 1, 1);

            var expected = new List<int>() {
                (0 << 6) + 0,
                (0 << 6) + 1,
                (0 << 6) + 2,

                (1 << 6) + 0,
                (1 << 6) + 1,
                (1 << 6) + 2,

                (2 << 6) + 0,
                (2 << 6) + 1,
                (2 << 6) + 2
            };
            Assert.AreEqual(expected, map.threateningAreas.FirstOrDefault(x => x.Item1 == creature.Id).Item2);
        }

        [Test]
        public void AddMediumCreatureWithReach()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(1, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", 2, new List<Damage>())
                    });
            map.AddCreature(creature, 1, 1);

            var expected = new List<int>() {
                (-1 << 6) + -1,
                (-1 << 6) + 0,
                (-1 << 6) + 1,
                (-1 << 6) + 2,
                (-1 << 6) + 3,

                (0 << 6) + -1,
                (0 << 6) + 0,
                (0 << 6) + 1,
                (0 << 6) + 2,
                (0 << 6) + 3,

                (1 << 6) + -1,
                (1 << 6) + 0,
                (1 << 6) + 1,
                (1 << 6) + 2,
                (1 << 6) + 3,

                (2 << 6) + -1,
                (2 << 6) + 0,
                (2 << 6) + 1,
                (2 << 6) + 2,
                (2 << 6) + 3,

                (3 << 6) + -1,
                (3 << 6) + 0,
                (3 << 6) + 1,
                (3 << 6) + 2,
                (3 << 6) + 3
            };
            Assert.AreEqual(expected, map.threateningAreas.FirstOrDefault(x => x.Item1 == creature.Id).Item2);
        }

        [Test]
        public void AddHugeCreatureWithBaseAttack()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(3, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", 1, new List<Damage>())
                    });
            map.AddCreature(creature, 0, 0);

            var expected = new List<int>() {
                (-1 << 6) + -1,
                (-1 << 6) + 0,
                (-1 << 6) + 1,
                (-1 << 6) + 2,
                (-1 << 6) + 3,

                (0 << 6) + -1,
                (0 << 6) + 0,
                (0 << 6) + 1,
                (0 << 6) + 2,
                (0 << 6) + 3,

                (1 << 6) + -1,
                (1 << 6) + 0,
                (1 << 6) + 1,
                (1 << 6) + 2,
                (1 << 6) + 3,

                (2 << 6) + -1,
                (2 << 6) + 0,
                (2 << 6) + 1,
                (2 << 6) + 2,
                (2 << 6) + 3,

                (3 << 6) + -1,
                (3 << 6) + 0,
                (3 << 6) + 1,
                (3 << 6) + 2,
                (3 << 6) + 3
            };
            Assert.AreEqual(expected, map.threateningAreas.FirstOrDefault(x => x.Item1 == creature.Id).Item2);
        }

        [Test]
        public void AddHugeCreatureWithReach()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(3, Loyalties.Ally,
                new List<Attack>() {
                    new Attack("stub", 2, new List<Damage>())
                    });
            map.AddCreature(creature, 0, 0);

            var expected = new List<int>() {

                (-2 << 6) + -2,
                (-2 << 6) + -1,
                (-2 << 6) + 0,
                (-2 << 6) + 1,
                (-2 << 6) + 2,
                (-2 << 6) + 3,
                (-2 << 6) + 4,

                (-1 << 6) + -2,
                (-1 << 6) + -1,
                (-1 << 6) + 0,
                (-1 << 6) + 1,
                (-1 << 6) + 2,
                (-1 << 6) + 3,
                (-1 << 6) + 4,

                (0 << 6) + -2,
                (0 << 6) + -1,
                (0 << 6) + 0,
                (0 << 6) + 1,
                (0 << 6) + 2,
                (0 << 6) + 3,
                (0 << 6) + 4,

                (1 << 6) + -2,
                (1 << 6) + -1,
                (1 << 6) + 0,
                (1 << 6) + 1,
                (1 << 6) + 2,
                (1 << 6) + 3,
                (1 << 6) + 4,

                (2 << 6) + -2,
                (2 << 6) + -1,
                (2 << 6) + 0,
                (2 << 6) + 1,
                (2 << 6) + 2,
                (2 << 6) + 3,
                (2 << 6) + 4,

                (3 << 6) + -2,
                (3 << 6) + -1,
                (3 << 6) + 0,
                (3 << 6) + 1,
                (3 << 6) + 2,
                (3 << 6) + 3,
                (3 << 6) + 4,

                (4 << 6) + -2,
                (4 << 6) + -1,
                (4 << 6) + 0,
                (4 << 6) + 1,
                (4 << 6) + 2,
                (4 << 6) + 3,
                (4 << 6) + 4,
            };
            Assert.AreEqual(expected, map.threateningAreas.FirstOrDefault(x => x.Item1 == creature.Id).Item2);
        }
    }
}
