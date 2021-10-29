using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core.Graph.Mocks.Creatures
{
    class StillCreature : ICreature
    {
        public Loyalties Loyalty => Loyalties.Ally;

        public Sizes Size => Sizes.Medium;

        public List<Speed> Movements => new List<Speed>()
        {
            new Speed(SpeedTypes.Walking, 0),
            new Speed(SpeedTypes.Swimming, -1)
        };

        public List<Attack> Attacks => new List<Attack>();

        public bool Disangaged => false;

        public int RolledInitiative => throw new System.NotImplementedException();

        public bool HasReaction()
        {
            return true;
        }
    }
}
