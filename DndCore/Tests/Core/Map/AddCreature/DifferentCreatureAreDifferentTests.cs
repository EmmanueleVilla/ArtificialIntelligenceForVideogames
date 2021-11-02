using Core.Map;
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
    class DifferentCreatureAreDifferentTests
    {
        [Test]
        public void TestDifferentIds()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            map.AddCreature(new MockedCreature(), 0, 0);
            map.AddCreature(new MockedCreature(), 1, 0);
            map.AddCreature(new MockedCreature(), 2, 0);
            map.AddCreature(new MockedCreature(), 3, 0);
            map.AddCreature(new MockedCreature(), 4, 0);
            map.AddCreature(new MockedCreature(), 5, 0);

            Assert.AreEqual(map.Creatures.Count(), map.Creatures.Select(x => x.Id).Distinct().Count());
        }
    }
}
