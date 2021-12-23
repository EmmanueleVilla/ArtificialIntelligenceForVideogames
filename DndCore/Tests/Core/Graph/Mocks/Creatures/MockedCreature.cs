using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using Moq;
using System;
using System.Collections.Generic;

namespace Tests.Core.Graph.Mocks.Creatures
{
    public class MockedCreature
    {
        public static ICreature Build(
            int size = 1,
            Loyalties loyalty = Loyalties.Ally,
            List<Attack> attacks = null,
            List<Speed> movements = null,
            bool disengaged = false,
            bool hasReactions = true)
        {
            int id = new Random().Next(0, int.MaxValue);
            var creature = new Mock<ICreature>();
            creature.Setup(x => x.Id).Returns(id);
            creature.Setup(x => x.Size).Returns(size);
            creature.Setup(x => x.Loyalty).Returns(loyalty);
            creature.Setup(x => x.RemainingMovement).Returns(movements);
            creature.Setup(x => x.Attacks).Returns(attacks);
            creature.Setup(x => x.Disangaged).Returns(disengaged);
            creature.Setup(x => x.ReactionUsed).Returns(!hasReactions);
            return creature.Object;
        }
    }
}
