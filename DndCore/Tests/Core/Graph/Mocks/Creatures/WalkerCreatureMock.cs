﻿using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;

namespace Tests.Core.Graph.Mocks
{
    class WalkerCreatureMock : ICreature
    {
        Sizes _size;
        public WalkerCreatureMock(Sizes size = Sizes.Medium)
        {
            _size = size;
        }
        public List<Speed> Movements =>
            new List<Speed>() {
                new Speed(SpeedTypes.Walking, 6)
            };

        public Sizes Size => _size;

        public List<Attack> Attacks => new List<Attack>();

        public Loyalties Loyalty => Loyalties.Ally;

        public bool Disangaged => false;

        public int RolledInitiative => throw new System.NotImplementedException();

        public bool HasReaction()
        {
            return true;
        }

        public int RollInitiative()
        {
            throw new NotImplementedException();
        }

        public AbilityScores AbilityScores => throw new NotImplementedException();
    }
}
