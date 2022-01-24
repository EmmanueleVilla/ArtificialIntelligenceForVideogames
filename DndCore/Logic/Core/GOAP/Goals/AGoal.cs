using Logic.Core.Battle;
using Logic.Core.Creatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.GOAP.Goals
{
    abstract class AGoal : IGoal
    {
        public float Modifier { get; set; } = 1.0f;

        public abstract float EvaluateGoal(ICreature creature, IDndBattle oldState, IDndBattle newState);
    }
}
