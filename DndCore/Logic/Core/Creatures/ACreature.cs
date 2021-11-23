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
        private readonly Random random;
        public int Id { get; private set; }

        protected ACreature(IDiceRoller roller = null, Random random = null)
        {
            this.roller = roller ?? DndModule.Get<IDiceRoller>();
            this.random = random ?? DndModule.Get<Random>();
            Id = this.random.Next(0, int.MaxValue);
        }

        //Implemented fields
        public int RolledInitiative { get; private set; }

        //Abstract fields
        public abstract Loyalties Loyalty { get; }
        public abstract Sizes Size { get; }
        public abstract List<Speed> Movements { get; set; }
        public abstract List<Attack> Attacks { get; }
        public abstract bool Disangaged { get; }

        public abstract int InitiativeModifier { get; }
        public abstract RollTypes InitiativeRollType { get; }

        public AbilityScores AbilityScores => GetAbilityScores();

        public bool HasAction { get; set; } = true;
        public bool HasBonusAction { get; set; } = true;
        public bool HasReaction { get; set; } = true;


        protected virtual AbilityScores GetAbilityScores()
        {
            return new AbilityScores(10, 10, 10, 10, 10, 10);
        }

        public int RollInitiative()
        {
            RolledInitiative = roller.Roll(InitiativeRollType, 1, 20, InitiativeModifier);
            return RolledInitiative;
        }

        public override bool Equals(object obj)
        {
            return Id == (obj as ICreature).Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
