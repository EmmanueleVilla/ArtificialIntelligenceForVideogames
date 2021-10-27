using Logic.Core.Actions;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class ElfFemaleWizard : ICreature
    {
        public Loyalties Loyalty => Loyalties.Ally;

        public Sizes Size => Sizes.Medium;

        public List<Speed> Movements => new List<Speed>() { new Speed(SpeedTypes.Walking, 6) };

        public List<Attack> Attacks => new List<Attack>();
    }
}
