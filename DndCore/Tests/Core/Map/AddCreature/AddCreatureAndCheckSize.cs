using Core.Map;
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
    class AddCreatureAndCheckSize
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
            var creature = new MockedCreature(Sizes.Medium, Loyalties.Ally);
            map.AddCreature(creature, 1, 1);

            var expected = new List<CellInfo>() {
                new CellInfo('G', 0, creature, 1, 1),
            };
            Assert.AreEqual(expected, map.occupiedCells);
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
            var creature = new MockedCreature(Sizes.Large, Loyalties.Ally);
            map.AddCreature(creature, 1, 1);

            var expected = new List<CellInfo>() {
                new CellInfo('G', 0, creature, 1, 1),
                new CellInfo('G', 0, creature, 1, 2),
                new CellInfo('G', 0, creature, 2, 1),
                new CellInfo('G', 0, creature, 2, 2),
            };
            Assert.AreEqual(expected, map.occupiedCells);
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
            var creature = new MockedCreature(Sizes.Huge, Loyalties.Ally);
            map.AddCreature(creature, 1, 1);

            var expected = new List<CellInfo>() {
                new CellInfo('G', 0, creature, 1, 1),
                new CellInfo('G', 0, creature, 1, 2),
                new CellInfo('G', 0, creature, 1, 3),
                new CellInfo('G', 0, creature, 2, 1),
                new CellInfo('G', 0, creature, 2, 2),
                new CellInfo('G', 0, creature, 2, 3),
                new CellInfo('G', 0, creature, 3, 1),
                new CellInfo('G', 0, creature, 3, 2),
                new CellInfo('G', 0, creature, 3, 3),
            };
            Assert.AreEqual(expected, map.occupiedCells);
        }
    }
}
