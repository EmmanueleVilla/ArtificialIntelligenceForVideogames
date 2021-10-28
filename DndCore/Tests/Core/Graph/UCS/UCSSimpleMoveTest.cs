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
    class UCSSimpleMoveTest
    {
        [Test]
        public void CanMoveOnlyOneSquare()
        {
            var mapCsv = "G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Medium);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var to = map.GetCellInfo(1, 0);
            var edge = new Edge(from, to, 1, 0, true);
            Assert.AreEqual(new List<Edge>() { edge }, new UniformCostSearch().Search(from, map));
        }

        [Test]
        public void CanMoveOnlyFromMiddleToTwoAdjiacentSquare()
        {
            var mapCsv = "G0,G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Medium);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 1, 0);
            var from = map.GetCellInfo(1, 0);
            var toOne = map.GetCellInfo(0, 0);
            var edgeOne = new Edge(from, toOne, 1, 0, true);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new Edge(from, toTwo, 1, 0, true);
            Assert.AreEqual(new List<Edge>() { edgeOne, edgeTwo }, new UniformCostSearch().Search(from, map));
        }

        [Test]
        public void CanMoveOnlyFromStartToTwoAdjiacentSquare()
        {
            var mapCsv = "G0,G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Medium);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var toOne = map.GetCellInfo(1, 0);
            var edgeOne = new Edge(from, toOne, 1, 0, true);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new Edge(from, toTwo, 2, 0, true);
            Assert.AreEqual(new List<Edge>() { edgeOne, edgeTwo }, new UniformCostSearch().Search(from, map));
        }

        [Test]
        public void CanMoveUntilMovementExpires()
        {
            var mapCsv = "G0,G0,G0,G0,G0,G0,G0,G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Medium);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var prev = from;
            var expected = new List<Edge>();
            for (int i = 1; i <= 6; i++)
            {
                var to = map.GetCellInfo(i, 0);
                var edge = new Edge(prev,to, i, 0, true);
                expected.Add(edge);
                prev = to;
            }

            Assert.AreEqual(expected, new UniformCostSearch().Search(map.GetCellInfo(0, 0), map));
        }

        [Test]
        public void CanMoveOnlyFromStartToTwoAdjiacentSquareButCantStopInTheMiddle()
        {
            var mapCsv = "G0,G0,G0";
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(new WalkerCreatureMock(Sizes.Medium), 0, 0);
            map.AddCreature(new WalkerCreatureMock(Sizes.Medium), 1, 0);
            var from = map.GetCellInfo(0, 0);
            var toOne = map.GetCellInfo(1, 0);
            var edgeOne = new Edge(from, toOne, 2, 0, false);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new Edge(from, toTwo, 3, 0, true);
            Assert.AreEqual(new List<Edge>() { edgeOne, edgeTwo }, new UniformCostSearch().Search(from, map));
        }
    }
}
