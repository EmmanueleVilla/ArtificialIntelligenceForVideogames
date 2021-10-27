using Core.Map;
using Logic.Core.Creatures;
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
    class SpeedCalculatorCanEndMovementHere: ASpeedCalculatorBaseTests
    {
        [Test]
        public void DestinationMapIsEmpty()
        {
            var creature = new WalkerCreatureMock();
            var to = new CellInfo('G', 0, null);
            Assert.AreEqual(true, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).CanEndMovementHere);
        }

        [Test]
        public void DestinationMapIsOccupiedByEnemy()
        {
            var creature = new WalkerCreatureMock();
            var to = new CellInfo('G', 0);
            Assert.AreEqual(false, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new OccupiedMap(Sizes.Gargantuan, Loyalties.Enemy)).CanEndMovementHere);
        }

        [Test]
        public void DestinationMapIsOccupiedByAlly()
        {
            var creature = new WalkerCreatureMock();
            var to = new CellInfo('G', 0);
            Assert.AreEqual(false, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new OccupiedMap(Sizes.Medium, Loyalties.Ally)).CanEndMovementHere);
        }
    }
}
