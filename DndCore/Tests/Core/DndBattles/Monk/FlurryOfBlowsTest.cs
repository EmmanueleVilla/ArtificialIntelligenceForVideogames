using Core.Map;
using Core.Utils.Log;
using Logic.Core;
using Logic.Core.Battle.Actions.Attacks;
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
using Tests.Core.Graph.Mocks.Creatures;

namespace Tests.Core.DndBattles.Monk
{
    [TestFixture]
    class FlurryOfBlowsTest
    {
        [Test]
        public void GetCurrentCreatureInTurn()
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
            map.AddCreature(monk, 1, 1);

            var enemy = new RatmanWithBow(new DiceRoller(), new Random());
            map.AddCreature(enemy, 2, 2);

            battle.Init(map);
            var creature = battle.GetCreatureInTurn();
            if(creature.Id != monk.Id) {
                battle.NextTurn();
            }

            var actions = battle.GetAvailableActions();
            Assert.True(actions.All(action => action.Description != "Flurry of blows"));
            battle.Attack(new ConfirmAttackAction()
            {
                Attack = monk.Attacks.First(attack => attack.Name.ToLower().Contains("unarmed")),
                TargetCreature = enemy,
                AttackingCreature = monk
            });

            actions = battle.GetAvailableActions();
            Assert.True(actions.First(action => action.Description == "Flurry of blows") != null);

            battle.Attack(new ConfirmAttackAction()
            {
                Attack = monk.Attacks.First(attack => attack.Name.ToLower().Contains("quarterstaff")),
                TargetCreature = enemy,
                AttackingCreature = monk
            });

            actions = battle.GetAvailableActions();
            Assert.True(actions.FirstOrDefault(action => action.Description.ToLower().Contains("flurry of blows")) != null);
            foreach(var action in actions)
            {
                Console.WriteLine(action.Description);
            }
            Assert.True(actions.FirstOrDefault(action => action.Description.ToLower().Contains("(b) unarmed")) != null);
        }
    }
}
