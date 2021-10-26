using Core.Map;
using NUnit.Framework;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorSwimming : ASpeedCalculatorBaseTests
    {
        [Test]
        public void SwimmingCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var to = new CellInfo('R', 0);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, CellInfo.Empty(), to).Speed);
        }

        [Test]
        public void SwimmingCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var to = new CellInfo('R', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, CellInfo.Empty(), to).Speed);
        }

        [Test]
        public void SwimmingCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var to = new CellInfo('R', 0);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, CellInfo.Empty(), to).Speed);
        }
    }
}
