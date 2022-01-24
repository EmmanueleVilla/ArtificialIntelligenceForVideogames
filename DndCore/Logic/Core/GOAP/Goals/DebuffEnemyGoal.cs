using Logic.Core.Battle;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    class DebuffEnemyGoal : AGoal
    {
        public override float EvaluateGoal(ICreature creature, IDndBattle oldState, IDndBattle newState)
        {
            return 0;
        }
    }
}
