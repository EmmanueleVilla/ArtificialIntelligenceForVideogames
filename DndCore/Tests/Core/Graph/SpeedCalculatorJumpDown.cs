using Core.Map;
using NUnit.Framework;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorJumpDown : ASpeedCalculatorBaseTests
    {
        // MOVEMENT TO A -1 LOWER CELL ---------------------------------------------------------

        [Test]
        public void OneLowerCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 1);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void OneLowerCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 1);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void OneLowerCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 1);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        // MOVEMENT TO A -2 LOWER CELL ---------------------------------------------------------

        [Test]
        public void TwoLowerCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 2);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void TwoLowerCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 2);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void TwoLowerCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 2);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        // MOVEMENT TO A -3 LOWER CELL ---------------------------------------------------------

        [Test]
        public void ThreeLowerCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 3);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(null, speedCalculator.GetNeededSpeed(character, from, to));
        }

        [Test]
        public void ThreeLowerCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 3);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(null, speedCalculator.GetNeededSpeed(character, from, to));
        }

        [Test]
        public void ThreeLowerCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 3);
            var to = new CellInfo('G', 0);
            Assert.AreEqual(null, speedCalculator.GetNeededSpeed(character, from, to));
        }
    }
}
