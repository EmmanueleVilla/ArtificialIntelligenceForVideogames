using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Battle.Actions.Abilities;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    class BuffAllyGoal : AGoal
    {
        public override float EvaluateGoal(ICreature creature, IDndBattle oldState, List<IAvailableAction> actions, IDndBattle newState)
        {
            var team = creature.Loyalty;
            var oldAllies = oldState.Map.Creatures.Where(x => x.Value.Loyalty == team);
            var oldBuffs = oldAllies.Count(x => x.Value.DodgeUsed);
            var newAllies = newState.Map.Creatures.Where(x => x.Value.Loyalty == team);
            var newBuffs = newAllies.Count(x => x.Value.DodgeUsed);
            var fullfillment = newBuffs - oldBuffs;
            if(actions.Any(x => x is DodgeAction))
            {
                fullfillment.ToString();
            }
            return fullfillment;
        }
    }
}
