using DndCore.Map;
using Logic.Core.Battle;
using Logic.Core.Creatures;
using Logic.Core.Graph;
using Logic.Core.Map;
using NUnit.Framework;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorHugeCreature: ASpeedCalculatorBaseTests
    {
        [Test]
        public void CanMoveSimpleWalking() {
            var creature = WalkerCreatureMock.Build(3);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,G0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from =  new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            var result = speedCalculator.GetNeededSpeed(creature, from, to, map);
            Assert.AreEqual(1, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(1, result.Speed);
        }

        [Test]
        public void CanMoveDifficultTerrain()
        {
            var creature = WalkerCreatureMock.Build(3);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,R0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            var result = speedCalculator.GetNeededSpeed(creature, from, to, map);
            Assert.AreEqual(1, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(2, result.Speed);
        }

        [Test]
        public void CanMoveDifficultTerrainCanSwim()
        {
            var creature = SwimmerCreatureMock.Build(1);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,R0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            var result = speedCalculator.GetNeededSpeed(creature, from, to, map);
            Assert.AreEqual(1, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(1, result.Speed);
        }

        [Test]
        public void CantMoveBecauseOfBlockedPath()
        {
            var creature = WalkerCreatureMock.Build(3);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,  ";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            var result = speedCalculator.GetNeededSpeed(creature, from, to, map);
            Assert.AreEqual(0, result.MovementEvents.Count);
            Assert.AreEqual(Edge.Empty(), speedCalculator.GetNeededSpeed(creature, from, to, map));
        }

        [Test]
        public void CanMoveBecauseHeightDifferenceAboveOnlyOne()
        {
            var creature = WalkerCreatureMock.Build(3);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,G1";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            var result = speedCalculator.GetNeededSpeed(creature, from, to, map);
            Assert.AreEqual(1, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(1, result.Speed);
        }

        [Test]
        public void CanMoveBecauseHeightDifferenceBelowOnlyOne()
        {
            var creature = WalkerCreatureMock.Build(3);

            var mapCsv = "" +
                "G1,G1,G1,  \n" +
                "G1,G1,G1,G1\n" +
                "G1,G1,G1,G1\n" +
                "  ,G1,G1,G0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            var result = speedCalculator.GetNeededSpeed(creature, from, to, map);
            Assert.AreEqual(1, result.MovementEvents.Count);
            Assert.AreEqual(GameEvent.Types.Movement, result.MovementEvents[0].Type);
            Assert.AreEqual(1, result.Speed);
        }

        [Test]
        public void CantMoveBecauseHeightDifferenceAboveTwo()
        {
            var creature = WalkerCreatureMock.Build(3);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,G2";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            var result = speedCalculator.GetNeededSpeed(creature, from, to, map);
            Assert.AreEqual(0, result.MovementEvents.Count);
            Assert.AreEqual(Edge.Empty(), result);
        }

        [Test]
        public void CantMoveBecauseHeightDifferenceBelowTwo()
        {
            var creature = WalkerCreatureMock.Build(3);

            var mapCsv = "" +
                "G2,G2,G2,  \n" +
                "G2,G2,G2,G2\n" +
                "G2,G2,G2,G2\n" +
                "  ,G2,G2,G0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            var result = speedCalculator.GetNeededSpeed(creature, from, to, map);
            Assert.AreEqual(0, result.MovementEvents.Count);
            Assert.AreEqual(Edge.Empty(), result);
        }
    }
}
