using Logic.Core.Actions;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Logic.Core.Creatures.Bestiary
{
    public class HumanMaleRanger : ICreature
    {
        public Loyalties Loyalty => Loyalties.Ally;

        public Sizes Size => Sizes.Medium;

        public List<Speed> Movements => new List<Speed>() { new Speed(SpeedTypes.Walking, 6) };

        public List<Attack> Attacks => new List<Attack>();
        public bool Disangaged => false;

        public bool HasReaction()
        {
            return true;
        }
    }
}
