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

        public Sizes Size => throw new System.NotImplementedException();

        public List<Attack> Attacks => throw new System.NotImplementedException();

        public Loyalties Loyalty => throw new System.NotImplementedException();
    }
}