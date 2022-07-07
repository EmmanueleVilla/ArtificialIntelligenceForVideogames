using DndCore.Map;
using Logic.Core.Creatures;
using Logic.Core.Map.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks;
using Tests.Core.Graph.Mocks.Creatures;

namespace Tests.Core.Map.AddCreature
{
    [TestFixture]
    class AddCreatureAndCheckSize: BaseDndTest
    {
        [Test]
        public void AddMediumCreature ()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(1, Loyalties.Ally);
            map.AddCreature(creature, 1, 1);

            var expected = new Dictionary<int, int>() {
                { (1 << 6) + 1, creature.Id }
            };
            Assert.AreEqual(expected, map.occupiedCellsDictionary);
        }

        [Test]
        public void AddLargeCreature()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(2, Loyalties.Ally);
            map.AddCreature(creature, 1, 1);

            var expected = new Dictionary<int, int>() {
                { (1 << 6) + 1, creature.Id },
                { (1 << 6) + 2, creature.Id },
                { (2 << 6) + 1, creature.Id },
                { (2 << 6) + 2, creature.Id }
            };
            Assert.AreEqual(expected, map.occupiedCellsDictionary);
        }

        [Test]
        public void AddHugeCreature()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = MockedCreature.Build(3, Loyalties.Ally);
            map.AddCreature(creature, 1, 1);

            var expected = new Dictionary<int, int>() {
                { (1 << 6) + 1, creature.Id },
                { (1 << 6) + 2, creature.Id },
                { (1 << 6) + 3, creature.Id },
                { (2 << 6) + 1, creature.Id },
                { (2 << 6) + 2, creature.Id },
                { (2 << 6) + 3, creature.Id },
                { (3 << 6) + 1, creature.Id },
                { (3 << 6) + 2, creature.Id },
                { (3 << 6) + 3, creature.Id },
            };
            Assert.AreEqual(expected, map.occupiedCellsDictionary);
        }
    }
}
