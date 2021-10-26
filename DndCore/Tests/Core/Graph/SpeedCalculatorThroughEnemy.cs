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
    class SpeedCalculatorThroughEnemy: ASpeedCalculatorBaseTests
    {
        [Test]
        [TestCase(Sizes.Tiny, Sizes.Tiny, false)]
        [TestCase(Sizes.Tiny, Sizes.Small, false)]
        [TestCase(Sizes.Tiny, Sizes.Medium, true)]
        [TestCase(Sizes.Tiny, Sizes.Large, true)]
        [TestCase(Sizes.Tiny, Sizes.Huge, true)]
        [TestCase(Sizes.Tiny, Sizes.Gargantuan, true)]

        [TestCase(Sizes.Small, Sizes.Tiny, false)]
        [TestCase(Sizes.Small, Sizes.Small, false)]
        [TestCase(Sizes.Small, Sizes.Medium, false)]
        [TestCase(Sizes.Small, Sizes.Large, true)]
        [TestCase(Sizes.Small, Sizes.Huge, true)]
        [TestCase(Sizes.Small, Sizes.Gargantuan, true)]

        [TestCase(Sizes.Medium, Sizes.Tiny, true)]
        [TestCase(Sizes.Medium, Sizes.Small, false)]
        [TestCase(Sizes.Medium, Sizes.Medium, false)]
        [TestCase(Sizes.Medium, Sizes.Large, false)]
        [TestCase(Sizes.Medium, Sizes.Huge, true)]
        [TestCase(Sizes.Medium, Sizes.Gargantuan, true)]

        [TestCase(Sizes.Large, Sizes.Tiny, true)]
        [TestCase(Sizes.Large, Sizes.Small, true)]
        [TestCase(Sizes.Large, Sizes.Medium, false)]
        [TestCase(Sizes.Large, Sizes.Large, false)]
        [TestCase(Sizes.Large, Sizes.Huge, false)]
        [TestCase(Sizes.Large, Sizes.Gargantuan, true)]

        [TestCase(Sizes.Huge, Sizes.Tiny, true)]
        [TestCase(Sizes.Huge, Sizes.Small, true)]
        [TestCase(Sizes.Huge, Sizes.Medium, true)]
        [TestCase(Sizes.Huge, Sizes.Large, false)]
        [TestCase(Sizes.Huge, Sizes.Huge, false)]
        [TestCase(Sizes.Huge, Sizes.Gargantuan, false)]
        public void CheckIfCreatureCanPassThroughtEnemy(Sizes mySize, Sizes enemySize, bool canPass)
        {
            var creature = new SizedCreature(mySize, Loyalties.Ally);
            var from = new CellInfo('G', 0);
            var to = new CellInfo('G', 0, new SizedCreature(enemySize, Loyalties.Enemy));
            var result = speedCalculator.GetNeededSpeed(creature, from, to, new OccupiedByEnemyMap(enemySize));
            if (canPass)
            {
                Assert.NotNull(result);
            } else
            {
                Assert.Null(result);
            }
        }
    }
}
