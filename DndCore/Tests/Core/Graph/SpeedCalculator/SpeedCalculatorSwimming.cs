using DndCore.Map;
using NUnit.Framework;
using Tests.Core.Graph.Mocks;
using Tests.Core.Graph.Mocks.Creatures;

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorSwimming : ASpeedCalculatorBaseTests
    {
        [Test]
        public void SwimmingCell_OnlyNormalSpeed()
        {
            var creature = WalkerCreatureMock.Build(1);
            var to = new CellInfo('R', 0);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Damage);
        }

        [Test]
        public void SwimmingCell_SwimmingSpeed()
        {
            var creature = SwimmerCreatureMock.Build(1);
            var to = new CellInfo('R', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Damage);
        }

        [Test]
        public void SwimmingCell_ClimbingSpeed()
        {
            var creature = ClimberCreatureMock.Build(1);
            var to = new CellInfo('R', 0);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Damage);
        }

        [Test]
        public void SwimmingCell_ExpiredSwimmingSpeed()
        {
            var creature = ExpiredSwimmerCreatureMock.Build(1);
            var to = new CellInfo('R', 0);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, CellInfo.Empty(), to, new EmptyMap()).Damage);
        }
    }
}
