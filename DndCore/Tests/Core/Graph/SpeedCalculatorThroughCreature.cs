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
    class SpeedCalculatorThroughCreature : ASpeedCalculatorBaseTests
    {
        [Test]
        public void DestinationMapIsEmpty()
        {
            var creature = new WalkerCreatureMock();
            var to = new CellInfo('G', 0, null);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
        }

        [Test]
        public void DestinationMapIsOccupiedByEnemy()
        {
            var creature = new WalkerCreatureMock();
            var to = new CellInfo('G', 0, new SizedCreature(Sizes.Gargantuan, Loyalties.Enemy));
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
        }

        [Test]
        public void DestinationMapIsOccupiedByAlly()
        {
            var creature = new WalkerCreatureMock();
            var to = new CellInfo('G', 0, new SizedCreature(Sizes.Medium, Loyalties.Ally));
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
        }
    }
}
