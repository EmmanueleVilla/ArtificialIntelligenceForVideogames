﻿using Core.DI;
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
    class UCSSimpleMoveTest
    {
        [SetUp]
        public void Setup()
        {
            DndModule.RegisterRulesForTest();
        }

        [Test]
        public void CanMoveOnlyOneSquare()
        {
            var mapCsv = "G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Medium);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var from = map.GetCellInfo(0, 0);
            var to = map.GetCellInfo(1, 0);
            var edge = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>(), to, 1, 0, true);
            var expected = new List<MemoryEdge>() { edge };
            var actual = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map);
            Assert.AreEqual(expected, actual);
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
            var edgeOne = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>(), toOne, 1, 0, true);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>(), toTwo, 1, 0, true);
            Assert.AreEqual(new List<MemoryEdge>() { edgeOne, edgeTwo }, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
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
            var edgeOne = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>(), toOne, 1, 0, true);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new MemoryEdge(new List<CellInfo>() { from, toOne }, new List<MovementEvent>(), toTwo, 2, 0, true);
            Assert.AreEqual(new List<MemoryEdge>() { edgeOne, edgeTwo }, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map));
        }

        [Test]
        public void CanMoveUntilMovementExpires()
        {
            var mapCsv = "G0,G0,G0,G0,G0,G0,G0,G0,G0";
            var creature = new WalkerCreatureMock(Sizes.Medium);
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(creature, 0, 0);
            var prev = new List<CellInfo>() { map.GetCellInfo(0, 0) };
            var expected = new List<MemoryEdge>();
            for (int i = 1; i <= 6; i++)
            {
                var to = map.GetCellInfo(i, 0);
                var edge = new MemoryEdge(new List<CellInfo>(prev), new List<MovementEvent>(), to, i, 0, true);
                expected.Add(edge);
                prev.Add(edge.Destination);
            }

            Assert.AreEqual(expected, new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(map.GetCellInfo(0, 0), map));
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
            var edgeOne = new MemoryEdge(new List<CellInfo>() { from }, new List<MovementEvent>(), toOne, 2, 0, false);
            var toTwo = map.GetCellInfo(2, 0);
            var edgeTwo = new MemoryEdge(new List<CellInfo>() { from, toOne }, new List<MovementEvent>(), toTwo, 3, 0, true);
            var expected = new List<MemoryEdge>() { edgeOne, edgeTwo };
            var actual = new UniformCostSearch(speedCalculator: new SpeedCalculator()).Search(from, map);
            Assert.AreEqual(expected, actual);
        }
    }
}
