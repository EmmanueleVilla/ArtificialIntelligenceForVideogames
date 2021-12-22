using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<Speed> _remainingMovement;

        protected ACreature(IDiceRoller roller = null, Random random = null)
        {
            this.roller = roller ?? DndModule.Get<IDiceRoller>();
            this.random = random ?? DndModule.Get<Random>();
            Id = this.random.Next(0, int.MaxValue);
            _remainingMovement = new List<Speed>(Movements);
            RemainingAttacksPerAction = AttacksPerAction;
            CurrentHitPoints = HitPoints;
            RemainingAttacksPerBonusAction = 0;
            RollInitiative();
        }
        
        public virtual void ResetTurn()
        {
            _remainingMovement = new List<Speed>(Movements);
            RemainingAttacksPerAction = AttacksPerAction;
            CurrentHitPoints = HitPoints;
            LastAttackUsed = null;
            RemainingAttacksPerBonusAction = 0;
            ActionUsedNotToAttack = false;
            ActionUsedToAttack = false;
            BonusActionUsedNotToAttack = false;
            BonusActionUsedToAttack = false;
            Disangaged = false;
        }

        //Abstract fields
        public abstract Loyalties Loyalty { get; }
        public abstract int Size { get; }
        public abstract int CriticalThreshold { get; }
        public abstract List<Attack> Attacks { get; }
        public abstract List<Speed> Movements { get; }
        public abstract int InitiativeModifier { get; }
        public abstract RollTypes InitiativeRollType { get; }
        public abstract int AttacksPerAction { get; }
        public abstract int HitPoints { get; }
        public abstract int ArmorClass { get; }
        public abstract AbilityScores AbilityScores { get; }

        //Implemented fields
        public virtual int RolledInitiative { get; private set; }
        public List<Speed> RemainingMovement {
            get {
                var minus = 0;
                if(TemporaryEffectsList.FirstOrDefault(x => x.Item3 == TemporaryEffects.SpeedReducedByTwo) != null)
                {
                    minus = 2;
                }
                return _remainingMovement.Select(x => new Speed(x.Item1, x.Item2 - minus)).ToList();
            }
            set {
                _remainingMovement = value;
            }
        }
        public bool Disangaged { get; set; }
        public int RemainingAttacksPerAction { get; set; }
        public bool ActionUsedNotToAttack { get; set; }
        public bool ActionUsedToAttack { get; set; }
        public bool BonusActionUsedNotToAttack { get; set; }
        public bool BonusActionUsedToAttack { get; set; }
        public bool ReactionUsed { get; set; }
        public int CurrentHitPoints { get; set; }
        public int TemporaryHitPoints { get; set; }
        public string LastAttackUsed { get; set; }
        public int RemainingAttacksPerBonusAction { get; set; }
        public List<Tuple<ICreature, int, TemporaryEffects>> TemporaryEffectsList { get; set; } = new List<Tuple<ICreature, int, TemporaryEffects>>();

        private int RollInitiative()
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
    }
}
