using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Tests.Core.Graph.Mocks
{
    class ExpiredClimberCreatureMock : ICreature
    {
        public List<Speed> Movements =>
            new List<Speed>() {
                new Speed(SpeedTypes.Walking, 10),
                new Speed(SpeedTypes.Climbing, 0)
            };

        public Sizes Size => Sizes.Medium;

        public List<Attack> Attacks => new List<Attack>();

        public Loyalties Loyalty => Loyalties.Ally;
    }
}