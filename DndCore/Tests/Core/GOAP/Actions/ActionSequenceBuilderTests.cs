using Core.Map;
using Core.Utils.Log;
using Logic.Core;
using Logic.Core.Battle.ActionBuilders;
using Logic.Core.Creatures.Bestiary;
using Logic.Core.Dice;
using Logic.Core.GOAP.Actions;
using Logic.Core.Graph;
using Logic.Core.Map.Impl;
using NUnit.Framework;
using System;
using Tests.Core.DndBattles.Mock;

namespace Tests.Core.GOAP.Actions
{
    [TestFixture]
    public class ActionSequenceBuilderTest
    {
        [Test]
        public void ActionSequenceBuilderBaseTest()
        {
            var battle = new DndBattle(new ZeroRoller(), new UniformCostSearch(
                new SpeedCalculator(), new ConsoleLogger()), new ActionBuildersWrapper(), new ConsoleLogger());
            var map = new ArrayDndMap(10, 10, CellInfo.Empty());
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var monk = new HumanFemaleMonk(new DiceRoller(new Random()), new Random());
            map.AddCreature(monk, 1, 1);

            var enemy = new RatmanWithBow(new DiceRoller(new Random()), new Random());
            map.AddCreature(enemy, 2, 2);

            battle.Init(map);

            var creature = battle.GetCreatureInTurn();
            if (creature.Id != monk.Id)
            {
                battle.NextTurn();
            }

            var builder = new ActionSequenceBuilder();
            builder.GetAvailableActions(battle);
        }
    }
}
