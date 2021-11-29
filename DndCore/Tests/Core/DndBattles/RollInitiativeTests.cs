using Core.DI;
using Core.Utils.Log;
using Logic.Core;
using Logic.Core.Graph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.DndBattles.Mock;

namespace Tests.Core.DndBattles
{
    [TestFixture]
    class RollInitiativeTests
    {
        [Test]
        public void GetCurrentCreatureInTurn()
        {
            var battle = new DndBattle(new UniformCostSearch(
                new SpeedCalculator(), new ConsoleLogger()));
            var expected = new List<int>() { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
            var map = new InitiativeListMap();
            var result = battle.Init(map).Select(x => x.RolledInitiative).ToList();
            Assert.AreEqual(expected, result);
        }
    }
}
