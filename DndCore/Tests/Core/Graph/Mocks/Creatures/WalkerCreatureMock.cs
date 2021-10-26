using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Tests.Core.Graph.Mocks
{
    class WalkerCreatureMock : ICreature
    {
        public List<Speed> Movements =>
            new List<Speed>() {
                new Speed(SpeedTypes.Walking, 6)
            };

        public Sizes Size => Sizes.Medium;

        public List<Attack> Attacks => throw new System.NotImplementedException();

        public Loyalties Loyalty => throw new System.NotImplementedException();
    }
}
