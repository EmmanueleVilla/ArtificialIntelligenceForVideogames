using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using Moq;
using System;
using System.Collections.Generic;

namespace Tests.Core.Graph.Mocks
{
    class ExpiredClimberCreatureMock
    {
        public static ICreature Build(int size = 1)
        {
            var creature = new Mock<ICreature>();
            creature.Setup(x => x.Size).Returns(size);
            creature.Setup(x => x.Loyalty).Returns(Loyalties.Ally);
            creature.Setup(x => x.RemainingMovement).Returns(new List<Speed>() {
                new Speed(SpeedTypes.Walking, 6),
                new Speed(SpeedTypes.Climbing, 0)
            });
            return creature.Object;
        }
    }
}
