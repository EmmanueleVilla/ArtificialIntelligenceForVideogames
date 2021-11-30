using Core.DI;
using Core.Map;
using Logic.Core.Battle;
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
            var creature = WalkerCreatureMock.Build(2);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var to = map.GetCellInfo(1, 0);
            var edge = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>()
            {
                new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1, 0)}
            }, to, 1, 0, true);
            Assert.AreEqual(new List<MemoryEdge>() { edge }, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
        }

        [Test]
        public void CanMoveOnlyOneSquareLeft()
        {
            var mapCsv =
                "G0,G0,G0\n" +
                "G0,G0,G0";
            var creature = WalkerCreatureMock.Build(2);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 1, 0);
            var from = map.GetCellInfo(1, 0);
            var to = map.GetCellInfo(0, 0);
            var edge = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>()
            {
                new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0, 0)}
            }, to, 1, 0, true);
            Assert.AreEqual(new List<MemoryEdge>() { edge }, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
        }

        [Test]
        public void CanMoveOnlyOneSquareDown()
        {
            var mapCsv =
                "G0,G0\n" +
                "G0,G0\n" +
                "G0,G0";
            var creature = WalkerCreatureMock.Build(2);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var to = map.GetCellInfo(0, 1);
            var edge = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>()
            {
                new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0, 1)}
            }, to, 1, 0, true);
            Assert.AreEqual(new List<MemoryEdge>() { edge }, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
        }

        [Test]
        public void CanMoveOnlyOneSquareUp()
        {
            var mapCsv =
                "G0,G0\n" +
                "G0,G0\n" +
                "G0,G0";
            var creature = WalkerCreatureMock.Build(2);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 1);
            var from = map.GetCellInfo(0, 1);
            var to = map.GetCellInfo(0, 0);
            var edge = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>()
            {
                new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0, 0)}
            }, to, 1, 0, true);
            Assert.AreEqual(new List<MemoryEdge>() { edge }, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
        }

        [Test]
        public void CanMoveOnlyFromMiddleToTwoAdjiacentSquare()
        {
            var mapCsv = 
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0";
            var creature = WalkerCreatureMock.Build(2);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 1, 0);
            var from = map.GetCellInfo(1, 0);
            var toOne = map.GetCellInfo(0, 0);
            var edgeOne = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>()
            {
                new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = toOne}
            }, toOne, 1, 0, true);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>(){
                new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = toTwo}
            }, toTwo, 1, 0, true);
            Assert.AreEqual(new List<MemoryEdge>() { edgeOne, edgeTwo }, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
        }
        [Test]
        public void CanMoveUntilMovementExpires()
        {
            var mapCsv = 
                "G0,G0,G0,G0,G0,G0,G0,G0,G0\n" +
                "G0,G0,G0,G0,G0,G0,G0,G0,G0";
            var creature = WalkerCreatureMock.Build(2);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var expected = new List<MemoryEdge>();
            var prev = new List<CellInfo>() { map.GetCellInfo(0, 0) };
            var events = new List<MovementEvent>();
            for (int i = 1; i <= 6; i++)
            {
                var to = map.GetCellInfo(i, 0);
                events.Add(new MovementEvent() { Type = MovementEvent.Types.Movement, Destination = to });
                var edge = new MemoryEdge(new List<CellInfo>(prev), new List<MovementEvent>(events), to, i, 0, true);
                expected.Add(edge);
                prev.Add(edge.Destination);
            }

            Assert.AreEqual(expected, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
        }

        [Test]
        public void CanMoveOnlyFromStartToTwoAdjiacentSquare()
        {
            var mapCsv =
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0";
            var creature = WalkerCreatureMock.Build(2);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var toOne = map.GetCellInfo(1, 0);
            var edgeOne = new MemoryEdge(new List<CellInfo> { from }, new List<MovementEvent>() {
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) }
                }, toOne, 1, 0, true);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new MemoryEdge(new List<CellInfo> { from, toOne }, new List<MovementEvent>() {
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) }
                }, toTwo, 2, 0, true);
            Assert.AreEqual(new List<MemoryEdge>() { edgeOne, edgeTwo }, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
        }

        [Test]
        public void CanMoveOnlyFromStartToTwoAdjiacentSquareButCantStopInTheMiddle()
        {
            var mapCsv =
                "G0,G0,G0,G0,G0\n" +
                "G0,G0,G0,G0,G0";
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(WalkerCreatureMock.Build(2), 0, 0);
            map.AddCreature(WalkerCreatureMock.Build(1), 2, 0);
            var from = map.GetCellInfo(0, 0);
            var toOne = map.GetCellInfo(1, 0);
            var edgeOne = new MemoryEdge(new List<CellInfo> { from }, new List<MovementEvent>() {
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) }
                }, toOne, 2, 0, false);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new MemoryEdge(new List<CellInfo> { from, toOne }, new List<MovementEvent>() {
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) }
                }, toTwo, 3, 0, false);
            var toThree = map.GetCellInfo(3, 0);
            var edgeThree = new MemoryEdge(new List<CellInfo> { from, toOne, toTwo }, new List<MovementEvent>() {
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(2,0) },
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(3,0) }
                }, toThree, 4, 0, true);
            Assert.AreEqual(new List<MemoryEdge>() { edgeOne, edgeTwo, edgeThree }, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
        }

        [Test]
        public void CantStopIfDoesntFit()
        {
            var mapCsv = "" +
                "G0,G0,G3,G3\n" +
                "G0,G0,G3,G3";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(WalkerCreatureMock.Build(2), 2, 0);
            var from = map.GetCellInfo(2, 0);
            var toOne = map.GetCellInfo(1, 0);
            var edgeOne = new MemoryEdge(new List<CellInfo> { from }, new List<MovementEvent>() {
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                    new MovementEvent { Type = MovementEvent.Types.Falling, FallingHeight = 1 }
                }, toOne, 3, 4, false);
            var toTwo = map.GetCellInfo(0, 0);
            var edgeTwo = new MemoryEdge(new List<CellInfo> { from, toOne }, new List<MovementEvent>() {
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(1,0) },
                    new MovementEvent { Type = MovementEvent.Types.Falling, FallingHeight = 1 },
                    new MovementEvent { Type = MovementEvent.Types.Movement, Destination = map.GetCellInfo(0,0) }
                }, toTwo, 4, 4, true);
            var expected = new List<MemoryEdge>() { edgeOne, edgeTwo };
            var actual = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map);
            Assert.AreEqual(expected, actual);

        }
    }
}
