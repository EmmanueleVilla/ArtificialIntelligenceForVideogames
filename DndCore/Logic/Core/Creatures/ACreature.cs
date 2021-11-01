using System;
using System.Collections.Generic;
using Core.DI;
using Logic.Core.Actions;
using Logic.Core.Creatures.Scores;
using Logic.Core.Dice;
using Logic.Core.Movements;

namespace Logic.Core.Creatures
{
    public abstract class ACreature : ICreature
    {
        private readonly IDiceRoller roller;

        protected ACreature(IDiceRoller roller = null)
        {
            this.roller = roller ?? DndModule.Get<IDiceRoller>();
        }

        //Implemented fields
        public int RolledInitiative { get; private set; }

        //Abstract fields
        public abstract Loyalties Loyalty { get; }
        public abstract Sizes Size { get; }
        public abstract List<Speed> Movements { get; }
        public abstract List<Attack> Attacks { get; }
        public abstract bool Disangaged { get; }
        public abstract bool HasReaction();

        public abstract int InitiativeModifier { get; }
        public abstract RollTypes InitiativeRollType { get; }

        public AbilityScores AbilityScores => GetAbilityScores();

        protected virtual AbilityScores GetAbilityScores()
        {
            return new AbilityScores(10, 10, 10, 10, 10, 10);
        }

        public int RollInitiative()
        {
            RolledInitiative = roller.Roll(InitiativeRollType, 1, 20, InitiativeModifier);
            return RolledInitiative;
        }
    }
}
