using Logic.Core.Actions;
using Logic.Core.Dice;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class DwarfMaleWarrior : ACreature
    {
        public override Loyalties Loyalty => Loyalties.Ally;

        public override Sizes Size => Sizes.Medium;

        public override List<Speed> Movements { get => new List<Speed>() { new Speed(SpeedTypes.Walking, 5) }; }

        public override List<Attack> Attacks => new List<Attack>();

        public override bool Disangaged => false;

        public override int InitiativeModifier => +3;

        public override RollTypes InitiativeRollType => RollTypes.Advantage;

    }
}
