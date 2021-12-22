using Core.Map;
using Core.Utils.Log;
using Logic.Core;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Bestiary;
using Logic.Core.Dice;
using Logic.Core.Graph;
using Logic.Core.Map.Impl;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.DndBattles.Mock;

namespace Tests.Core.DndBattles.TemporaryEffectsTests
{
    [TestFixture]
    class OneTurnExpirationEffect
    {
        [Test]
        public void ExpiresAtTheBeginningOfMyNextTurn()
        {
            var battle = new DndBattle(new ZeroRoller(), new UniformCostSearch(
                new SpeedCalculator(), new ConsoleLogger()));
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var monk = new HumanFemaleMonk(new DiceRoller(), new Random());
            monk.Init();
            map.AddCreature(monk, 1, 1);

            var enemy = new RatmanWithBow(new DiceRoller(), new Random());
            enemy.Init();
            map.AddCreature(enemy, 2, 2);

            battle.Init(map);
            var creature = battle.GetCreatureInTurn();
            if (creature.Id != monk.Id)
            {
                battle.NextTurn();
            }

            //This is the monk
            creature = battle.GetCreatureInTurn();
            battle.UseAbility(new PatientDefenseAction());

            Assert.AreEqual(1, creature.TemporaryEffectsList.Count);
            Assert.AreEqual(creature.Id, creature.TemporaryEffectsList[0].Item1);
            Assert.AreEqual(1, creature.TemporaryEffectsList[0].Item2);
            Assert.AreEqual(TemporaryEffects.DisadvantageToSufferedAttacks, creature.TemporaryEffectsList[0].Item3);

            battle.NextTurn();

            Assert.AreEqual(1, creature.TemporaryEffectsList.Count);
            Assert.AreEqual(creature.Id, creature.TemporaryEffectsList[0].Item1);
            Assert.AreEqual(1, creature.TemporaryEffectsList[0].Item2);
            Assert.AreEqual(TemporaryEffects.DisadvantageToSufferedAttacks, creature.TemporaryEffectsList[0].Item3);

            battle.NextTurn();
            Assert.AreEqual(0, creature.TemporaryEffectsList.Count);
        }
    }
}
