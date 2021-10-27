﻿using Core.Map;
using Logic.Core.Creatures;
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
            var creature = new WalkerCreatureMock(Sizes.Huge);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,G0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from =  new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, from, to, map).Speed);
        }

        [Test]
        public void CanMoveDifficultTerrain()
        {
            var creature = new WalkerCreatureMock(Sizes.Huge);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,R0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, from, to, map).Speed);
        }

        [Test]
        public void CanMoveDifficultTerrainCanSwim()
        {
            var creature = new SwimmerCreatureMock(Sizes.Huge);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,R0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, from, to, map).Speed);
        }

        [Test]
        public void CantMoveBecauseOfBlockedPath()
        {
            var creature = new WalkerCreatureMock(Sizes.Huge);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,  ";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            Assert.AreEqual(null, speedCalculator.GetNeededSpeed(creature, from, to, map));
        }

        [Test]
        public void CanMoveBecauseHeightDifferenceAboveOnlyOne()
        {
            var creature = new WalkerCreatureMock(Sizes.Huge);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,G1";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, from, to, map).Speed);
        }

        [Test]
        public void CanMoveBecauseHeightDifferenceBelowOnlyOne()
        {
            var creature = new WalkerCreatureMock(Sizes.Huge);

            var mapCsv = "" +
                "G1,G1,G1,  \n" +
                "G1,G1,G1,G1\n" +
                "G1,G1,G1,G1\n" +
                "  ,G1,G1,G0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, from, to, map).Speed);
        }

        [Test]
        public void CantMoveBecauseHeightDifferenceAboveTwo()
        {
            var creature = new WalkerCreatureMock(Sizes.Huge);

            var mapCsv = "" +
                "G0,G0,G0,  \n" +
                "G0,G0,G0,G0\n" +
                "G0,G0,G0,G0\n" +
                "  ,G0,G0,G2";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            Assert.AreEqual(null, speedCalculator.GetNeededSpeed(creature, from, to, map));
        }

        [Test]
        public void CantMoveBecauseHeightDifferenceBelowTwo()
        {
            var creature = new WalkerCreatureMock(Sizes.Huge);

            var mapCsv = "" +
                "G2,G2,G2,  \n" +
                "G2,G2,G2,G2\n" +
                "G2,G2,G2,G2\n" +
                "  ,G2,G2,G0";

            var map = new CsvFullMapBuilder().FromCsv(mapCsv);

            var from = new CellInfo('G', 0, creature, 0, 0);
            var to = new CellInfo('G', 0, null, 1, 1);
            Assert.AreEqual(null, speedCalculator.GetNeededSpeed(creature, from, to, map));
        }
    }
}