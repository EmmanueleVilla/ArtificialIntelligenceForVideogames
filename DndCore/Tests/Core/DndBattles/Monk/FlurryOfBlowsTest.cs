using Core.DI;
using Core.Map;
using Core.Utils.Log;
using Logic.Core;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Battle.Actions.Attacks;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Abilities;
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
        public void FlurryOfBlowsAvailableAndNumberOfBonusAttacks()
        {
            DndModule.RegisterRules();
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
            if(creature.Id != monk.Id) {
                battle.NextTurn();
            }
            battle.BuildAvailableActions();
            var actions = battle.GetAvailableActions();
            Assert.True(actions.All(action => action.Description != "Flurry of blows"));

            // First attack with unarmed
            var result = battle.Attack(new ConfirmAttackAction()
            {
                Attack = monk.Attacks.First(attack => attack.Name.ToLower().Contains("unarmed")),
                TargetCreature = enemy.Id,
                AttackingCreature = monk.Id
            });

            battle.BuildAvailableActions();
            actions = battle.GetAvailableActions();

            // I can't use Flurry of blows
            Assert.True(actions.First(action => action.Description == "Flurry of blows") != null);

            // Second attack with quarterstaff
            result = battle.Attack(new ConfirmAttackAction()
            {
                Attack = monk.Attacks.First(attack => attack.Name.ToLower().Contains("quarterstaff")),
                TargetCreature = enemy.Id,
                AttackingCreature = monk.Id
            });

            battle.BuildAvailableActions();
            actions = battle.GetAvailableActions();

            // I can use Flurry of blows
            Assert.True(actions.FirstOrDefault(action => action.Description.ToLower().Contains("flurry of blows")) != null);

            // I can make another attack as bonus action
            Assert.True((monk as IMartialArts).BonusAttackTriggered);
            Assert.True(actions.FirstOrDefault(action => action.Description.ToLower().Contains("unarmed") && action.ActionEconomy == BattleActions.BonusAction) != null);

            result = battle.Attack(new ConfirmAttackAction()
            {
                Attack = monk.Attacks.First(attack => attack.Name.ToLower().Contains("unarmed")),
                TargetCreature = enemy.Id,
                AttackingCreature = monk.Id,
                ActionEconomy = BattleActions.BonusAction
            });

            battle.BuildAvailableActions();
            actions = battle.GetAvailableActions();

            // I don't have other bonus action attacks
            Assert.True(actions.FirstOrDefault(action => action.Description.ToLower().Contains("unarmed") && action.ActionEconomy == BattleActions.BonusAction) == null);

            result = battle.UseAbility(new FlurryOfBlowsAction());

            battle.BuildAvailableActions();
            actions = battle.GetAvailableActions();

            // Now I have an additional bonus attack
            Assert.True(actions.FirstOrDefault(action => action.Description.ToLower().Contains("unarmed") && action.ActionEconomy == BattleActions.BonusAction) != null);

            result = battle.Attack(new ConfirmAttackAction()
            {
                Attack = monk.Attacks.First(attack => attack.Name.ToLower().Contains("unarmed")),
                TargetCreature = enemy.Id,
                AttackingCreature = monk.Id,
                ActionEconomy = BattleActions.BonusAction
            });

            battle.BuildAvailableActions();
            actions = battle.GetAvailableActions();

            // And no more
            Assert.True(actions.FirstOrDefault(action => action.Description.ToLower().Contains("(b) unarmed")) == null);

            var battleCopy = battle.Copy();
            battleCopy.BuildAvailableActions();
            actions = battleCopy.GetAvailableActions();

            // Also the copy can't attack
            Assert.True(actions.FirstOrDefault(action => action.Description.ToLower().Contains("(b) unarmed")) == null);
        }
    }
}
