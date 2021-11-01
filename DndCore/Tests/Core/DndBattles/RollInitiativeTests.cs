using Logic.Core;
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
            var battle = new DndBattle();
            battle.Init(new InitiativeListMap());
            var expected = new List<int>() { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
            var result = battle.RollInitiative().Select(x => x.RolledInitiative).ToList();
            Assert.AreEqual(expected, result);
        }
    }
}
