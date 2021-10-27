using Logic.Core.Actions;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public abstract class ARatman : ICreature
    {
        public Loyalties Loyalty => Loyalties.Enemy;

        public Sizes Size => Sizes.Medium;

        public List<Speed> Movements => new List<Speed>() { new Speed(SpeedTypes.Walking, 6) };

        public abstract List<Attack> Attacks { get; }
    }
}
