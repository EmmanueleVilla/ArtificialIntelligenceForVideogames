﻿using Core.Map;
using NUnit.Framework;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorWalking: ASpeedCalculatorBaseTests
    {

        [Test]
        public void NormalCell_OnlyNormalSpeed()
        {
            var creature = new WalkerCreatureMock();
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Damage);
        }

        [Test]
        public void NormalCell_SwimmingSpeed()
        {
            var creature = new SwimmerCreatureMock();
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Damage);
        }

        [Test]
        public void NormalCell_ClimbingSpeed()
        {
            var creature = new ClimberCreatureMock();
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Damage);
        }
    }
}
