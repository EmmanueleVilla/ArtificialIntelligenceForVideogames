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
    internal class IncreaseAllyHPGoal : AGoal
    {
        public override float EvaluateGoal(ICreature creature, IDndBattle oldState, List<IAvailableAction> actions, IDndBattle newState)
        {
            var team = creature.Loyalty;
            var allies = oldState.Map.Creatures.Where(x => x.Value.Loyalty == team);
            var oldHp = allies.Sum(x => x.Value.CurrentHitPoints);
            var newAllies = newState.Map.Creatures.Where(x => x.Value.Loyalty == team);
            var newHp = newAllies.Sum(x => x.Value.CurrentHitPoints);
            var fullfillment = newHp - oldHp;
            if(fullfillment < 0)
            {
                fullfillment *= 10;
            }
            var writeToFile = DndModule.Get<WriteToFileBool>();
            if (writeToFile.ShouldWrite)
            {
                File.AppendAllText("log.txt", "Increase ally hp: " + fullfillment + "\n");
            }
            return fullfillment;
        }
    }
}
