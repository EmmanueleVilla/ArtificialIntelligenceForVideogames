using Core.Map;
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
            var character = new WalkerCreatureMock();
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, CellInfo.Empty(), to).Speed);
        }

        [Test]
        public void NormalCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, CellInfo.Empty(), to).Speed);
        }

        [Test]
        public void NormalCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, CellInfo.Empty(), to).Speed);
        }
    }
}
