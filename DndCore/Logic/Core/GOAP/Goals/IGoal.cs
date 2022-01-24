using Logic.Core.Battle;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    interface IGoal
    {
        float Modifier { get; set; }
        float EvaluateGoal(ICreature creature, IDndBattle oldState, IDndBattle newState);
    }
}
