using Logic.Core.Actions;
using Logic.Core.Dice;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public abstract class ARatman : ACreature
    {
        public override Loyalties Loyalty => Loyalties.Enemy;

        public override Sizes Size => Sizes.Medium;

        public override int CriticalThreshold => 20;

        public override List<Speed> Movements => new List<Speed>()
        {
            new Speed(SpeedTypes.Walking, 6)
        };

        public override RollTypes InitiativeRollType => RollTypes.Normal;

        public override int AttacksPerAction => 1;
    }
}
