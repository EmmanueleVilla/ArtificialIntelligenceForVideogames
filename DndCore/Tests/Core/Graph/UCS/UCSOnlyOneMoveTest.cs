using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Graph;
using Logic.Core.Map;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph.UCS
{
    [TestFixture]
    class UCSOnlyOneMoveTest
    {
        [Test]
        public void CanMoveOnlyOneSquare()
        {
            var mapCsv = "G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Medium);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var to = map.GetCellInfo(1, 0);
            var edge = new Edge(to, 1, 0, true);
            Assert.AreEqual(new List<Edge>() { edge }, new UniformCostSearch().Search(map.GetCellInfo(0,0), map));
        }
    }
}
