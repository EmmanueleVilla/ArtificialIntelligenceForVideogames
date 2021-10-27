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
    class AddCreatureTest
    {
        [Test]
        public void AddCreatureAndRetrieve()
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
    }
}
