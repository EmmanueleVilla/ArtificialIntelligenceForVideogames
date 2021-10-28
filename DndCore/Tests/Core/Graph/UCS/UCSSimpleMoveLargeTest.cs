using Core.DI;
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
    class UCSSimpleMoveLargeTest
    {
        [SetUp]
        public void Setup()
        {
            DndModule.RegisterRulesForTest();
        }
        [Test]
        public void CanMoveOnlyOneSquareRight()
        {
            var mapCsv = 
                "G0,G0,G0\n" +
                "G0,G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Large);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var to = map.GetCellInfo(1, 0);
            var edge = new Edge(from, to, 1, 0, true);
            Assert.AreEqual(new List<Edge>() { edge }, new UniformCostSearch().Search(from, map));
        }

        [Test]
        public void CanMoveOnlyOneSquareLeft()
        {
            var mapCsv =
                "G0,G0,G0\n" +
                "G0,G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Large);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 1, 0);
            var from = map.GetCellInfo(1, 0);
            var to = map.GetCellInfo(0, 0);
            var edge = new Edge(from, to, 1, 0, true);
            Assert.AreEqual(new List<Edge>() { edge }, new UniformCostSearch().Search(from, map));
        }

        [Test]
        public void CanMoveOnlyOneSquareDown()
        {
            var mapCsv =
                "G0,G0\n" +
                "G0,G0\n" +
                "G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Large);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var to = map.GetCellInfo(0, 1);
            var edge = new Edge(from, to, 1, 0, true);
            Assert.AreEqual(new List<Edge>() { edge }, new UniformCostSearch().Search(from, map));
        }

        [Test]
        public void CanMoveOnlyOneSquareUp()
        {
            var mapCsv =
                "G0,G0\n" +
                "G0,G0\n" +
                "G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Large);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 1);
            var from = map.GetCellInfo(0, 1);
            var to = map.GetCellInfo(0, 0);
            var edge = new Edge(from, to, 1, 0, true);
            Assert.AreEqual(new List<Edge>() { edge }, new UniformCostSearch().Search(from, map));
        }

        [Test]
        public void CanMoveOnlyFromMiddleToTwoAdjiacentSquare()
        {
            var mapCsv = 
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Large);
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
            var mapCsv =
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Large);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var toOne = map.GetCellInfo(1, 0);
            var edgeOne = new Edge(from, toOne, 1, 0, true);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new Edge(toOne, toTwo, 2, 0, true);
            Assert.AreEqual(new List<Edge>() { edgeOne, edgeTwo }, new UniformCostSearch().Search(from, map));
        }

        [Test]
        public void CanMoveUntilMovementExpires()
        {
            var mapCsv = 
                "G0,G0,G0,G0,G0,G0,G0,G0,G0\n" +
                "G0,G0,G0,G0,G0,G0,G0,G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Large);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var expected = new List<Edge>();
            var prev = from;
            for (int i = 1; i <= 6; i++)
            {
                var to = map.GetCellInfo(i, 0);
                var edge = new Edge(prev, to, i, 0, true);
                expected.Add(edge);
                prev = to;
            }

            Assert.AreEqual(expected, new UniformCostSearch().Search(from, map));
        }

        [Test]
        public void CanMoveOnlyFromStartToTwoAdjiacentSquareButCantStopInTheMiddle()
        {
            var mapCsv =
                "G0,G0,G0,G0,G0\n" +
                "G0,G0,G0,G0,G0";
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(new WalkerCreatureMock(Sizes.Large), 0, 0);
            map.AddCreature(new WalkerCreatureMock(Sizes.Medium), 2, 0);
            var toOne = map.GetCellInfo(1, 0);
            var from = map.GetCellInfo(0, 0);
            var edgeOne = new Edge(from, toOne, 2, 0, false);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new Edge(toOne, toTwo, 3, 0, true);
            var toThree = map.GetCellInfo(3, 0);
            var edgeThree = new Edge(toTwo, toThree, 4, 0, true);
            Assert.AreEqual(new List<Edge>() { edgeOne, edgeTwo, edgeThree }, new UniformCostSearch().Search(from, map));
        }
    }
}
