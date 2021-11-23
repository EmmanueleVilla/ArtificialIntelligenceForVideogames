using Logic.Core.Actions;
using Logic.Core.Creatures;
using Logic.Core.Creatures.Scores;
using Logic.Core.Movements;
using System;
using System.Collections.Generic;

namespace Tests.Core.Graph.Mocks
{
    class ExpiredClimberCreatureMock : ICreature
    {
        public int Id { get; set; }
        public List<Speed> Movements
        {
            get => new List<Speed>() {
                new Speed(SpeedTypes.Walking, 10),
                new Speed(SpeedTypes.Climbing, 0)
            };
            set => throw new Exception("NYI");
        }

        public Sizes Size => Sizes.Medium;

        public List<Attack> Attacks => new List<Attack>();

        public Loyalties Loyalty => Loyalties.Ally;

        public bool Disangaged => false;

        public int RolledInitiative => throw new System.NotImplementedException();

        public int RollInitiative()
        {
            throw new NotImplementedException();
        }

        public AbilityScores AbilityScores => throw new NotImplementedException();

        public bool HasAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool HasBonusAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool HasReaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}