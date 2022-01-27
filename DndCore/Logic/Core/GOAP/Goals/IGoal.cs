using Logic.Core.Battle;
using Logic.Core.Battle.Actions;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    public interface IGoal
    {
        float Modifier { get; set; }
        float EvaluateGoal(ICreature creature, IDndBattle oldState, List<IAvailableAction> actions, IDndBattle newState);
    }
}
