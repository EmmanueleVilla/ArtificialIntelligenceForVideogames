using Core.Map;
using Logic.Core.Creatures;
using Logic.Core.Graph;
using NUnit.Framework;
using Tests.Core.Graph.Mocks;

namespace Tests.Core.Graph
{
    [TestFixture]
    public class SpeedCalculatorTests
    {
        SpeedCalculator speedCalculator = new SpeedCalculator();

        // MOVEMENT TO A NORMAL SPEED CELL ---------------------------------------------------------

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

        // MOVEMENT TO A SWIMMING SPEED CELL ---------------------------------------------------------

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

        // MOVEMENT TO A +1 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void OneHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void OneHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void OneHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        // MOVEMENT TO A +2 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void TwoHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 2);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void TwoHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 2);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void TwoHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 2);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        // MOVEMENT TO A +3 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void ThreeHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 3);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void ThreeHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 3);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void ThreeHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 3);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        // MOVEMENT TO A +4 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void FourHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 4);
            Assert.AreEqual(4, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void FourHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 4);
            Assert.AreEqual(4, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void FourHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 4);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        // MOVEMENT TO A +5 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void FiveHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 5);
            Assert.AreEqual(5, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void FiveHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 5);
            Assert.AreEqual(5, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void FiveHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 5);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        // MOVEMENT TO A +6 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void SixHigherCell_OnlyNormalSpeed()
        {
            var character = new WalkerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(6, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void SixHigherCell_SwimmingSpeed()
        {
            var character = new SwimmerCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(6, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

        [Test]
        public void SixHigherCell_ClimbingSpeed()
        {
            var character = new ClimberCreatureMock();
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(character, from, to).Speed);
        }

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
