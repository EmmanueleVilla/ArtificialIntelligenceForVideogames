using Logic.Core.Battle;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    internal class IncreaseAllyHPGoal : AGoal
    {
        public override float EvaluateGoal(ICreature creature, IDndBattle oldState, IDndBattle newState)
        {
            var team = creature.Loyalty;
            var allies = oldState.Map.Creatures.Where(x => x.Value.Loyalty == team);
            var oldHp = allies.Sum(x => x.Value.CurrentHitPoints + x.Value.TemporaryHitPoints);
            var newHp = allies.Sum(x => x.Value.CurrentHitPoints + x.Value.TemporaryHitPoints);
            return newHp - oldHp;
        }
    }
}
