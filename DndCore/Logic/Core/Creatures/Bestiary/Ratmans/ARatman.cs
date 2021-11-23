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

        public override List<Speed> Movements { get => new List<Speed>() { new Speed(SpeedTypes.Walking, 6) }; set => throw new NotImplementedException(); }

        public override abstract List<Attack> Attacks { get; }

        public override bool Disangaged => false;

        public override RollTypes InitiativeRollType => RollTypes.Normal;
    }
}
