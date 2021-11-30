using Core.Map;
using NUnit.Framework;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorJumpDown : ASpeedCalculatorBaseTests
    {
        [Test]
        public void OneLowerCell()
        {
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 1);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void TwoLowerCell()
        {
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 2);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(4, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void ThreeLowerCell()
        {
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 3);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(4, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void FourLowerCell()
        {
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 4);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(4, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(8, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }
    }
}
