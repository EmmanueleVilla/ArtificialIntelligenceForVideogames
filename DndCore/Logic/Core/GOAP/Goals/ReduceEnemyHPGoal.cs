using Logic.Core.Battle;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    class ReduceEnemyHPGoal : AGoal
    {
        public override float EvaluateGoal(ICreature creature, IDndBattle oldState, IDndBattle newState)
        {
            var team = creature.Loyalty;
            var enemies = oldState.Map.Creatures.Where(x => x.Value.Loyalty != team);
            var oldHp = enemies.Sum(x => x.Value.CurrentHitPoints + x.Value.TemporaryHitPoints);
            var newHp = enemies.Sum(x => x.Value.CurrentHitPoints + x.Value.TemporaryHitPoints);
            return oldHp - newHp;
        }
    }
}
