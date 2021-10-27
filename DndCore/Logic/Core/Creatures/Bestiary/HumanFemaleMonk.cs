using Logic.Core.Actions;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Core.Creatures.Bestiary
{
    public class HumanFemaleMonk : ICreature
    {
        public Loyalties Loyalty => Loyalties.Ally;

        public Sizes Size => Sizes.Medium;

        public List<Speed> Movements => new List<Speed>() { new Speed(SpeedTypes.Walking, 8) };

        public List<Attack> Attacks => new List<Attack>();
        public bool Disangaged => false;

        public bool HasReaction()
        {
            return true;
        }
    }
}
