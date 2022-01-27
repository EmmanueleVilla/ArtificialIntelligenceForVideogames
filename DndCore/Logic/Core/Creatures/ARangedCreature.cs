using Logic.Core.Dice;
using Logic.Core.GOAP.Goals;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures
{
    public abstract class ARangedCreature : ACreature
    {
        protected ARangedCreature(IDiceRoller roller = null, Random random = null) : base(roller, random)
        {

        }
        public override List<IGoal> goals => new List<IGoal>() {
                new IncreaseAllyHPGoal(),
                new ReduceEnemyHPGoal(),
                new BuffAllyGoal(),
                new DontWasteResourcesGoal(),
                new RangedPositionGoal()
            };
    }
}
