using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Core.Graph.Mocks
{
    class SwimmerCreatureMock
    {
        public static ICreature Build(int size = 1)
        {
            var creature = new Mock<ICreature>();
            creature.Setup(x => x.Size).Returns(size);
            creature.Setup(x => x.Loyalty).Returns(Loyalties.Ally);
            creature.Setup(x => x.RemainingMovement).Returns(new List<Speed>() {
                new Speed(SpeedTypes.Walking, 6),
                new Speed(SpeedTypes.Swimming, 8)
            });
            return creature.Object;
        }
    }
}
