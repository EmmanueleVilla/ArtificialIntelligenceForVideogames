﻿using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
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
        public int Id { get; set; }
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

        public int RollInitiative()
        {
            throw new NotImplementedException();
        }

        public AbilityScores AbilityScores => throw new NotImplementedException();

        public bool HasAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool HasBonusAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool ICreature.HasReaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
