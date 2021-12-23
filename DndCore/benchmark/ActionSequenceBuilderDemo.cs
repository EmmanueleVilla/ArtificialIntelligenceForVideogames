using Core.DI;
using Core.Map;
using Core.Utils.Log;
using Logic.Core;
using Logic.Core.Battle.ActionBuilders;
using Logic.Core.Creatures.Bestiary;
using Logic.Core.Dice;
using Logic.Core.GOAP.Actions;
using Logic.Core.Graph;
using Logic.Core.Map.Impl;
using Logic.Core.Utils.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Core.DndBattles.Mock;

namespace Benchmark
{
    public class ActionSequenceBuilderDemo
    {
        public ActionSequenceBuilderDemo()
        {
            DndModule.RegisterRules(false);
            var battle = new DndBattle(new AlwaysHitRoller(), new UniformCostSearch(
                new SpeedCalculator(), new NoLogger()), new ActionBuildersWrapper(), new NoLogger());
            var map = new ArrayDndMap(20, 20, CellInfo.Empty());
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    map.SetCell(i, j, new CellInfo('G', 0, null, i, j));
                }
            }
            var monk = new HumanFemaleMonk(new DiceRoller(new Random()), new Random());
            monk.Init();
            map.AddCreature(monk, 1, 1);

            var enemy = new RatmanWithBow(new DiceRoller(new Random()), new Random());
            enemy.Init();
            map.AddCreature(enemy, 5, 5);

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
