using System;
using System.Collections.Generic;
using Core.DI;
using Logic.Core.Actions;
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
        public int RolledInitiative {
            get {
                if (_rolledInitiative == null) {
                    _rolledInitiative = roller.Roll(InitiativeRollType, 1, 20, InitiativeModifier);
                }
                return _rolledInitiative.Value;
            }
        }


        //Abstract fields
        public abstract Loyalties Loyalty { get; }
        public abstract Sizes Size { get; }
        public abstract List<Speed> Movements { get; }
        public abstract List<Attack> Attacks { get; }
        public abstract bool Disangaged { get; }
        public abstract bool HasReaction();

        public abstract int InitiativeModifier { get; }
        public abstract RollTypes InitiativeRollType { get; }

        //Common logic
        private int? _rolledInitiative = null;
    }
}
