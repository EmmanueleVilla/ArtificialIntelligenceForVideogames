﻿using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Movements;
using System.Collections.Generic;

namespace Tests.Core.Graph.Mocks
{
    class ClimberCreatureMock : ICreature
    {
        public List<Speed> Movements =>
            new List<Speed>() {
                new Speed(SpeedTypes.Walking, 10),
                new Speed(SpeedTypes.Climbing, 8)
            };

        public Sizes Size => Sizes.Medium;

        public List<Attack> Attacks => new List<Attack>();

        public Loyalties Loyalty => Loyalties.Ally;

        public bool Disangaged => false;

        public int RolledInitiative => throw new System.NotImplementedException();

        public bool HasReaction()
        {
            return true;
        }
    }
}