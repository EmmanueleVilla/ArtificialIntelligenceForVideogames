using Core.Map;
using NUnit.Framework;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph
{
    [TestFixture]
    class SpeedCalculatorClimbing : ASpeedCalculatorBaseTests
    {
        // MOVEMENT TO A +1 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void OneHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void OneHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void OneHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        // MOVEMENT TO A +2 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void TwoHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 2);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void TwoHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 2);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void TwoHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 2);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        // MOVEMENT TO A +3 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void ThreeHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 3);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void ThreeHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 3);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void ThreeHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 3);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        // MOVEMENT TO A +4 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void FourHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 4);
            Assert.AreEqual(4, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void FourHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 4);
            Assert.AreEqual(4, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void FourHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 4);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        // MOVEMENT TO A +5 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void FiveHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 5);
            Assert.AreEqual(5, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void FiveHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 5);
            Assert.AreEqual(5, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void FiveHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 5);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        // MOVEMENT TO A +6 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void SixHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(6, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void SixHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(6, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }

        [Test]
        public void SixHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(character, from, to, new EmptyMap()).Speed);
        }
    }
}
