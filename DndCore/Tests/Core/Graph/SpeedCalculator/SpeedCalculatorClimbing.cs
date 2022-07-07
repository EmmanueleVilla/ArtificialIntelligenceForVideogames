using DndCore.Map;
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
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void OneHigherCell_SwimmingSpeed()
        {
            var creature = SwimmerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void OneHigherCell_ClimbingSpeed()
        {
            var creature = ClimberCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 1);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        // MOVEMENT TO A +2 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void TwoHigherCell_OnlyNormalSpeed()
        {
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 2);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void TwoHigherCell_SwimmingSpeed()
        {
            var creature = SwimmerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 2);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void TwoHigherCell_ClimbingSpeed()
        {
            var creature = ClimberCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 2);
            Assert.AreEqual(1, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        // MOVEMENT TO A +3 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void ThreeHigherCell_OnlyNormalSpeed()
        {
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 3);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void ThreeHigherCell_SwimmingSpeed()
        {
            var creature = SwimmerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 3);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void ThreeHigherCell_ClimbingSpeed()
        {
            var creature = ClimberCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 3);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        // MOVEMENT TO A +4 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void FourHigherCell_OnlyNormalSpeed()
        {
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 4);
            Assert.AreEqual(4, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void FourHigherCell_SwimmingSpeed()
        {
            var creature = SwimmerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 4);
            Assert.AreEqual(4, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void FourHigherCell_ClimbingSpeed()
        {
            var creature = ClimberCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 4);
            Assert.AreEqual(2, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        // MOVEMENT TO A +5 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void FiveHigherCell_OnlyNormalSpeed()
        {
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 5);
            Assert.AreEqual(5, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void FiveHigherCell_SwimmingSpeed()
        {
            var creature = SwimmerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 5);
            Assert.AreEqual(5, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void FiveHigherCell_ClimbingSpeed()
        {
            var creature = ClimberCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 5);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        // MOVEMENT TO A +6 HIGHER CELL ---------------------------------------------------------

        [Test]
        public void SixHigherCell_OnlyNormalSpeed()
        {
            var creature = WalkerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(6, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void SixHigherCell_SwimmingSpeed()
        {
            var creature = SwimmerCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(6, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void SixHigherCell_ClimbingSpeed()
        {
            var creature = ClimberCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(3, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }

        [Test]
        public void SixHigherCell_ExpiredClimbingSpeed()
        {
            var creature = ExpiredClimberCreatureMock.Build(1);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 6);
            Assert.AreEqual(6, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Speed);
            Assert.AreEqual(0, speedCalculator.GetNeededSpeed(creature, from, to, new EmptyMap()).Damage);
        }
    }
}
