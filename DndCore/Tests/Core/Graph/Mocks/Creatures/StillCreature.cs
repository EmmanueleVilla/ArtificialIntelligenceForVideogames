﻿using Logic.Core.Actions;
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
        public Loyalties Loyalty => throw new NotImplementedException();

        public Sizes Size => throw new NotImplementedException();

        public List<Speed> Movements => new List<Speed>()
        {
            new Speed(SpeedTypes.Walking, 0),
            new Speed(SpeedTypes.Swimming, -1)
        };

        public List<Attack> Attacks => throw new NotImplementedException();
    }
}