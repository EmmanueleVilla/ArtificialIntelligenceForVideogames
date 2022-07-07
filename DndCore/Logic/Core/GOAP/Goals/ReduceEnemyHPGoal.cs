using DndCore.DI;
using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static DndCore.DI.DndModule;

namespace Logic.Core.GOAP.Goals
{
    class ReduceEnemyHPGoal : AGoal
    {
        public override float EvaluateGoal(ICreature creature, IDndBattle oldState, List<IAvailableAction> actions, IDndBattle newState)
        {
            var team = creature.Loyalty;
            var enemies = oldState.Map.Creatures.Where(x => x.Value.Loyalty != team);
            var oldHp = enemies.Sum(x => x.Value.CurrentHitPoints + x.Value.TemporaryHitPoints);
            var newEnemies = newState.Map.Creatures.Where(x => x.Value.Loyalty != team);
            var newHp = newEnemies.Sum(x => x.Value.CurrentHitPoints + x.Value.TemporaryHitPoints);
            var fullfillment = oldHp - newHp;
            var writeToFile = DndModule.Get<WriteToFileBool>();
            if (writeToFile.ShouldWrite)
            {
                File.AppendAllText("log.txt", "Reduce enemy hp: " + fullfillment + "\n");
            }
            return fullfillment;
        }
    }
}
