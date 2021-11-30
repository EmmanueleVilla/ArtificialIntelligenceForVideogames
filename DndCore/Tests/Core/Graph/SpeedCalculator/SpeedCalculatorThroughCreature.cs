using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Map;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.Graph.Mocks;
using Tests.Core.Graph.Mocks.Creatures;

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorThroughCreature : ASpeedCalculatorBaseTests
    {
        [Test]
        public void DestinationMapIsEmpty()
        {
            var creature = WalkerCreatureMock.Build(1);
            var to = new CellInfo('G', 0, null);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
        }

        [Test]
        public void DestinationMapIsOccupiedByRootEnemy()
        {
            var creature = WalkerCreatureMock.Build(1);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new OccupiedMap(4, Loyalties.Enemy)).Speed);
        }

        [Test]
        public void DestinationMapIsOccupiedByNonRootEnemy()
        {
            var creature = WalkerCreatureMock.Build(1);
            var mapCsv = "" +
                " ,G,G,G\n" +
                "G,G,G,G\n" +
                " ,G,G,G";
            var map = new CsvFullMapBuilder().FromCsv(mapCsv);
            map.AddCreature(MockedCreature.Build(1, Loyalties.Ally), 0, 1);
            map.AddCreature(MockedCreature.Build(3, Loyalties.Enemy), 1, 0);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(
                creature, map.GetCellInfo(0,1),
                map.GetCellInfo(1, 0),
                map).Speed);
        }

        [Test]
        public void DestinationMapIsOccupiedByAlly()
        {
            var creature = WalkerCreatureMock.Build(1);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new OccupiedMap(1, Loyalties.Ally)).Speed);
        }
    }
}
