using Core.Map;
using Logic.Core.Map.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Map
{
    [TestFixture]
    class AddCreatureGetTest
    {
        [Test]
        public void AddCreatureAndRetrieveCell()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for(int i = 0; i<10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            map.AddCreature(new WalkerCreatureMock(), 1, 1);
            Assert.NotNull(map.GetCellInfo(1, 1).Creature);
        }

        [Test]
        public void AddCreatureAndRetrieveList()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var creature = new WalkerCreatureMock();
            map.AddCreature(creature, 1, 1);
            Assert.AreEqual(1, map.Creatures.Count);
            Assert.AreEqual(creature, map.Creatures[0]);
        }

        [Test]
        public void AddCreaturesAndRetrieveList()
        {
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var one = new WalkerCreatureMock();
            map.AddCreature(one, 1, 1);
            var two = new WalkerCreatureMock();
            map.AddCreature(two, 2, 2);
            Assert.AreEqual(2, map.Creatures.Count);
            Assert.AreEqual(one, map.Creatures[0]);
            Assert.AreEqual(two, map.Creatures[1]);
        }
    }
}
